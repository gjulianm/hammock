﻿using System;
using System.IO;
using System.Net;
using Hammock.Extensions;

#if SILVERLIGHT || PORTABLE || METRO
using Hammock.Silverlight.Compat;
#else
using System.Collections.Specialized;
#endif

namespace Hammock
{
#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class RestResponseBase : IDisposable
    {
        private string _content;
        public virtual string Content
        {
            get
            {
                if(_content == null && ContentStream != null)
                {
                    ContentStream = ReplaceContentStreamWithMemoryStream();
                    using (var reader = new StreamReader(ContentStream))
                    {
                        _content = reader.ReadToEnd();
                    }
                    if (ContentStream.CanSeek)
                    {
                        ContentStream.Position = 0;
                    }
                }
                return _content;
            }
        }

        private byte[] _contentBytes;
        public virtual byte[] ContentBytes
        {
            get
            {
                if(_contentBytes == null && ContentStream != null)
                {
                    ContentStream = ReplaceContentStreamWithMemoryStream();
                    _contentBytes = ReadFully(ContentStream);
                    if(ContentStream.CanSeek)
                    {
                        ContentStream.Position = 0;
                    }
                }
                return _contentBytes;
            }
        }

        public virtual object ErrorContentEntity { get; set; }
        public virtual Stream ContentStream { get; set; }
        public virtual WebResponse InnerResponse { get; set; }
        public virtual Exception InnerException { get; set; }
        public virtual DateTime? RequestDate { get; set; }
        public virtual DateTime? ResponseDate { get; set; }
        public virtual string RequestMethod { get; set; }
        public virtual bool RequestKeptAlive { get; set; }
        public virtual HttpStatusCode StatusCode { get; set; }
        public virtual string StatusDescription { get; set; }
        public virtual string ContentType { get; set; }
        public virtual long ContentLength { get; set; }
        public virtual Uri RequestUri { get; set; }
        public virtual Uri ResponseUri { get; set; }
        public virtual bool IsMock { get; set; }
        public virtual bool TimedOut { get; set; }
        public virtual int TimesTried { get; set; }
        public virtual object Tag { get; set; }
        public virtual NameValueCollection Headers { get; set; }
        [Obsolete("Use CookieContainer instead.")]
        public virtual NameValueCollection Cookies { get; set; }
#if !NETCF
        public virtual CookieContainer CookieContainer { get; set; }
#endif
        public virtual bool SkippedDueToRateLimitingRule { get; set; }
        public virtual bool IsFromCache
        {
            get
            {
                return StatusCode == 0 && 
                       StatusDescription.IsNullOrBlank() && 
                       Content != null;
            }
        }

        public RestResponseBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            Headers = new NameValueCollection(0);
#pragma warning disable 618
            Cookies = new NameValueCollection(0);
#pragma warning restore 618
        }

        // http://www.yoda.arachsys.com/csharp/readbinary.html
        public static byte[] ReadFully(Stream stream)
        {
            const int initialLength = 32768;
            var buffer = new byte[initialLength];
            var read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;
                if (read != buffer.Length)
                {
                    continue;
                }
                var nextByte = stream.ReadByte();
                if (nextByte == -1)
                {
                    return buffer;
                }
                var newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }
            var ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        private Stream ReplaceContentStreamWithMemoryStream()
        {
            if(ContentStream is DurableMemoryStream)
            {
                return ContentStream;
            }

            var buffer = new byte[4096];
            var stream = new MemoryStream();
            var count = 0;
            do
            {
                if (ContentStream == null)
                {
                    continue;
                }
                count = ContentStream.Read(buffer, 0, buffer.Length);
                stream.Write(buffer, 0, count);
            } while (count != 0);

            if (ContentStream != null)
            {
#if !PORTABLE
                ContentStream.Close();
#endif
                ContentStream.Dispose();
            }

            if(stream.CanSeek)
            {
                stream.Position = 0;
            }

            return new DurableMemoryStream(stream);
        }

        private class DurableMemoryStream : MemoryStream
        {
            private readonly Stream _stream;

            public DurableMemoryStream(Stream stream)
            {
                _stream = stream;
            }

#if !METRO
            public override IAsyncResult BeginRead(byte[] buffer, int offset, int count,
                                                   AsyncCallback callback, object state)
            {
                return _stream.BeginRead(buffer, offset, count, callback, state);
            }
#else

            public async void BeginRead(byte[] buffer, int offset, int count,
                                                   AsyncCallback callback, object state)
            {
                await _stream.ReadAsync(buffer, offset, count);
                callback(new AwaitAsyncResult(state));
            }
#endif

#if !METRO
            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count,
                                                    AsyncCallback callback, object state)
            {
                return _stream.BeginWrite(buffer, offset, count, callback, state);
            }
#else
            public async void BeginWrite(byte[] buffer, int offset, int count,
                                                    AsyncCallback callback, object state)
            {
                await _stream.WriteAsync(buffer, offset, count);
                callback(new AwaitAsyncResult(state));
            }
#endif

            public override bool CanRead
            {
                get { return _stream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return _stream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return _stream.CanWrite; }
            }

#if !METRO
            public override void Close()
            {
                _stream.Flush();
            }
#else
            public void Close()
            {
                _stream.Flush();
            }
#endif

#if !METRO
            public override int EndRead(IAsyncResult asyncResult)
            {
                return _stream.EndRead(asyncResult);
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                _stream.EndWrite(asyncResult);
            }
#endif

            public override void Flush()
            {
                _stream.Flush();
            }

            public override long Length
            {
                get
                {
                    return _stream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    return _stream.Position;
                }
                set
                {
                    _stream.Position = value;
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _stream.Read(buffer, offset, count);
            }

            public override int ReadByte()
            {
                return _stream.ReadByte();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _stream.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                _stream.WriteByte(value);
            }

            protected override void Dispose(bool disposing)
            {
                if(disposing)
                {
                    _stream.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        public void SetContent(string content)
        {
            _content = content;
        }

        public void Dispose()
        {
            if(ContentStream != null)
            {
                ContentStream.Dispose();
            }
        }
    }

#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class RestResponse : RestResponseBase
    {
        public virtual object ContentEntity { get; set; }
    }

#if !SILVERLIGHT && !PORTABLE
    [Serializable]
#endif
    public class RestResponse<T> : RestResponseBase
    {
        public virtual T ContentEntity { get; set; }
    }
}


