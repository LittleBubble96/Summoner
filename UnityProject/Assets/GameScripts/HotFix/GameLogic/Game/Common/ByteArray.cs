using System;
using System.IO;
using System.Text;

namespace Config
{
    public class ByteArray : IDisposable
    {
        private MemoryStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public ByteArray()
        {
            _stream = new MemoryStream();
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }

        public ByteArray(int buffSize)
        {
            _stream = new MemoryStream(buffSize);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);
        }

        public ByteArray(byte[] buff)
        {
            _stream = new MemoryStream(buff.Length);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);

            _stream.Write(buff, 0, buff.Length);
            _stream.Position = 0;
        }

        public ByteArray(byte[] buff, int start, int size)
        {
            _stream = new MemoryStream(size);
            _reader = new BinaryReader(_stream);
            _writer = new BinaryWriter(_stream);

            _stream.Write(buff, start, size);
            _stream.Position = 0;
        }

        public bool ReadAvailable
        {
            get { return _stream.Length > _stream.Position; }
        }

        public int Position
        {
            get { return (int) _stream.Position; }
            set { _stream.Position = value; }
        }

        public byte[] Bytes
        {
            get { return _stream.ToArray(); }
        }
        
        public void Seek(int offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }


        //--------------------------------------------------------------------------------
        // 读取-小端
        //--------------------------------------------------------------------------------

        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        public bool ReadBool()
        {
            return _reader.ReadBoolean();
        }

        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        public short ReadShort()
        {
            return _reader.ReadInt16();
        }

        public ushort ReadUshort()
        {
            return _reader.ReadUInt16();
        }

        public int ReadInt()
        {
            return _reader.ReadInt32();
        }

        public uint ReadUint()
        {
            return _reader.ReadUInt32();
        }

        public long ReadLong()
        {
            return _reader.ReadInt64();
        }

        public ulong ReadUlong()
        {
            return _reader.ReadUInt64();
        }

        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        public double ReadDouble()
        {
            return _reader.ReadDouble();
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }
        
        public void ReadVector3(out float x, out float y, out float z)
        {
            x = _reader.ReadSingle();
            y = _reader.ReadSingle();
            z = _reader.ReadSingle();
        }

        public string ReadUTF()
        {
            int size = ReadInt();
            return Encoding.UTF8.GetString(_reader.ReadBytes(size));
        }

        public byte[] ReadBytes(int length)
        {
            return _reader.ReadBytes(length);
        }


        //--------------------------------------------------------------------------------
        // 写入-小端
        //--------------------------------------------------------------------------------

        public void WriteByte(byte value)
        {
            _writer.Write(value);
        }
        
        public void WriteBytes(byte[] value)
        {
            _writer.Write(value);
        }

        public void WriteSByte(sbyte value)
        {
            _writer.Write(value);
        }

        public void WriteShort(short value)
        {
            _writer.Write(value);
        }

        public void WriteUshort(ushort value)
        {
            _writer.Write(value);
        }

        public void WriteInt(int value)
        {
            _writer.Write(value);
        }

        public void WriteUint(uint value)
        {
            _writer.Write(value);
        }

        public void WriteLong(long value)
        {
            _writer.Write(value);
        }

        public void WriteUlong(ulong value)
        {
            _writer.Write(value);
        }

        public void WriteString(string value)
        {
            _writer.Write(value);
        }

        public void WriteStream(MemoryStream stream)
        {
            var buff = stream.ToArray();
            _writer.Write(buff);
        }

        public void WriteFloat(float value)
        {
            _writer.Write(value);
        }

        public void WriteBool(bool value)
        {
            _writer.Write(value);
        }

        public void WriteVector3(float x, float y, float z)
        {
            _writer.Write(x);
            _writer.Write(y);
            _writer.Write(z);
        }


        //--------------------------------------------------------------------------------
        // 读取-大端
        //--------------------------------------------------------------------------------

        public byte ReadByteB()
        {
            return _reader.ReadByte();
        }

        public short ReadShortB()
        {
            byte[] bytes = new byte[sizeof(short)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToInt16(bytes, 0);
        }

        public int ReadIntB()
        {
            byte[] bytes = new byte[sizeof(int)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        public long ReadLongB()
        {
            byte[] bytes = new byte[sizeof(long)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToInt64(bytes, 0);
        }

        public ushort ReadUShortB()
        {
            byte[] bytes = new byte[sizeof(ushort)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToUInt16(bytes, 0);
        }

        public uint ReadUIntB()
        {
            byte[] bytes = new byte[sizeof(uint)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        public ulong ReadULongB()
        {
            byte[] bytes = new byte[sizeof(ulong)];
            for (int i = bytes.Length - 1; i > -1; --i)
            {
                bytes[i] = _reader.ReadByte();
            }

            return BitConverter.ToUInt64(bytes, 0);
        }


        //--------------------------------------------------------------------------------
        // 写入-大端
        //--------------------------------------------------------------------------------

        public void WriteByteB(byte value)
        {
            _writer.Write(value);
        }

        public void WriteShortB(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void WriteIntB(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void WriteLongB(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void WriteUShortB(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void WriteUIntB(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        public void WriteULongB(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            _writer.Write(bytes);
        }

        #region IDisposable Support

        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (null != _stream)
                    {
                        _stream.Dispose();
                        _stream = null;
                    }

                    if (null != _reader)
                    {
                        _reader.Close();
                        _reader = null;
                    }

                    if (null != _writer)
                    {
                        _writer.Close();
                        _writer = null;
                    }
                }

                _stream = null;
                _reader = null;
                _writer = null;

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}