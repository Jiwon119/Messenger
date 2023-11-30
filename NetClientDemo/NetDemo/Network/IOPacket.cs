using System;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using System.Text;
using NetDemo.Network.NetObjects;

namespace NetDemo.Network.IOPacket
{
    
    public interface IPacketSerializable
    {
        void ReadPacket(InPacket iPacket);
        void WritePacket(OutPacket oPacket);
    }

    public class InOutPacket
    {    
        public InOutPacket()
        {
            _buffer = new byte[InitialSize];
            _capacity = InitialSize;
        }
        protected readonly int InitialSize = 1024;
        internal byte[] _buffer = null;
        protected int _curIdx = 0;
        protected int _endIdx = 0;
        protected int _capacity = 0;

        protected int _packetHeader = -1;
        protected int _packetSize = -1;
        
        public int PacketID { get => _packetHeader; }

        public int Capacity
        {
            get { return _capacity - HeaderSize; }
        }
        public int Length
        {
            get { return _endIdx; }
        }
        public int HeaderSize
        {
            get { return 8; } // 일단은 임시
        }
        public int CurPos
        { 
            get { return _curIdx; }
        }
        public int BodySize
        {
            get => _endIdx - HeaderSize;
        }


        public virtual void Clear()
        {
            _curIdx = 0;
            _endIdx = 0;
        }
        public void ExpendPacket()
        {
            byte[] newBuff = new byte[_capacity * 2];
            Buffer.BlockCopy(_buffer, 0, newBuff, 0, _capacity);
            _buffer = newBuff;
            _capacity = newBuff.Length;
        }

        public bool MakeFromBuffer(byte[] buff, int count)
        {
            if (null == buff || 0 >= count)
                return false;

            while (count >= Capacity) ExpendPacket();

            Buffer.BlockCopy(buff, 0, _buffer, 0, count);
            _endIdx = count;
            _curIdx = count;
            return true;
        }

        public byte[] ExtractData(int start, int end)
        {
            if (0 > start || _curIdx <= start)
                return null;

            if (0 > end || _curIdx <= end)
                return null;

            if (start >= end)
                return null;

            byte[] res = new byte[end - start];
            Buffer.BlockCopy(_buffer, start, res, 0, end - start);
            return res;
        }
        public byte[] ExtractData(int start)
        {
            return ExtractData(start, _curIdx - 1);
        }
        public void ImportData(byte[] data, int start)
        {
            if (null == data || 0 >= data.Length)
                return;

            if (0 > start || _curIdx < start)
                return;

            while (data.Length >= Capacity) ExpendPacket();
            Buffer.BlockCopy(data, 0, _buffer, start, data.Length);
            _curIdx += data.Length;
        }
        public void ImportData(byte[] data)
        {
            ImportData(data, _curIdx);
        }
    }

    public class InPacket : InOutPacket
    {
        public InPacket()
        {
            Clear();
        }
        
        public bool IsReadComplete
        { 
            get { return (_curIdx == _endIdx); }
        }


        public override void Clear()
        {
            _packetHeader = -1;
            _packetSize = -1;
            _curIdx = HeaderSize;
            _endIdx = HeaderSize;
        }

        public void Reset()
        {
            _curIdx = HeaderSize;
        }

        public bool ParsePacket(RecvPacketBuffer recv_stream)
        {
            if (recv_stream == null)
            {
                return false;
            }

            // 일단 header 부터 읽어본다.
            // header는 id - 4 byte, size - 4byte
            if (recv_stream.LeftDataSize < HeaderSize)
            {
                // 버퍼에 남은 데이터의 크기가 header보다 작다면 덜 받아진 것.
                // 다음 회차에 마저 읽어준다.
                return false;
            }

            Buffer.BlockCopy(recv_stream.GetBuffer, recv_stream.CurPos, _buffer, 0, HeaderSize);
            _packetHeader = BitConverter.ToInt32(_buffer, 0);
            _packetSize = BitConverter.ToInt32(_buffer, 4);

            //header를 읽어봤더니 패킷 전체 사이즈가 나왔다.
            //header + body = _packetSize
            if (recv_stream.LeftDataSize < _packetSize)
            {
                // body까지 읽기엔 버퍼에 데이터가 적음
                // 다음 회차에 읽어준다.
                return false;
            }

            int LeftReadSize = _packetSize - HeaderSize;

            while (true)
            {
                if (LeftReadSize < Capacity)
                    break;

                ExpendPacket();
            }

            recv_stream.CurPos = recv_stream.CurPos + HeaderSize;
            Buffer.BlockCopy(recv_stream.GetBuffer, recv_stream.CurPos, 
                _buffer, HeaderSize, LeftReadSize);
            recv_stream.CurPos = recv_stream.CurPos + LeftReadSize;

            _endIdx += LeftReadSize;
            _curIdx = HeaderSize;
            return true;
        }
        public bool ParsePacket(OutPacket oPacket)
        {
            Clear();
            MakeFromBuffer(oPacket.GetBuffer, oPacket.Length);
            _packetHeader = BitConverter.ToInt32(_buffer, 0);
            _packetSize = BitConverter.ToInt32(_buffer, 4);
            return true;
        }

        public void Copy(out InPacket iPacket)
        {
            iPacket = PacketPool.Get.GetInPacket();
            iPacket.MakeFromBuffer(_buffer, _endIdx);
        }

        #region byte to data method
        private int CheckReadPossible(int size)
        {
            int left = _endIdx - _curIdx;
            if (left < size)
                throw new InvalidPacketException(string.Format(
                    "InPacket Read out of index - size: {0}, left: {1}",
                    size, left));

            return size;
        }

        public void ReadPacket(out int val)
        {
            val = BitConverter.ToInt32(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(int));
        }
        public void ReadPacket(out uint val)
        {
            val = BitConverter.ToUInt32(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(uint));
        }
        public void ReadPacket(out short val)
        {
            val = BitConverter.ToInt16(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(short));
        }
        public void ReadPacket(out ushort val)
        {
            val = BitConverter.ToUInt16(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(ushort));
        }
        public void ReadPacket(out long val)
        {
            val = BitConverter.ToInt64(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(long));
        }
        public void ReadPacket(out ulong val)
        {
            val = BitConverter.ToUInt64(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(ulong));
        }
        public void ReadPacket(out float val)
        {
            val = BitConverter.ToSingle(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(float));
        }
        public void ReadPacket(out double val)
        {
            val = BitConverter.ToDouble(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(double));
        }
        public void ReadPacket(out bool val)
        {
            val = BitConverter.ToBoolean(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(bool));
        }
        public void ReadPacket(out char val)
        {
            val = BitConverter.ToChar(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(char));
        }
        public void ReadPacket(out byte val)
        {
            val = _buffer[_curIdx];
            _curIdx += CheckReadPossible(sizeof(byte));
        }
        public void ReadPacket(out string val)
        {
            int size = ReadInt32();
            CheckReadPossible(size);
            val = Encoding.Default.GetString(_buffer, _curIdx, size);
            _curIdx += size;
        }
        public void ReadPacket(out byte[] val)
        {
            int size = ReadInt32();
            CheckReadPossible(size);
            val = new byte[size];
            Buffer.BlockCopy(_buffer, _curIdx, val, 0, size);
            _curIdx += size;
        }
        public void ReadPacket<T>(out T data) where T : struct, IPacketSerializable
        {
            data = new T();
            data.ReadPacket(this);            
        }
        public void ReadPacket<T>(T data) where T : class, IPacketSerializable
        {
            data.ReadPacket(this);
        }

        //exception 처리 해야함.
        public int ReadInt32()
        {
            int val = BitConverter.ToInt32(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(int));
            return val;
        }

        public uint ReadUint32()
        {
            uint val = BitConverter.ToUInt32(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(uint));
            return val;
        }
        public short ReadInt16()
        {
            short val = BitConverter.ToInt16(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(short));
            return val;
        }
        public ushort ReadUint16()
        {
            ushort val = BitConverter.ToUInt16(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(ushort));
            return val;
        }
        public long ReadInt64()
        {
            long val = BitConverter.ToInt64(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(long));
            return val;
        }
        public ulong ReadUint64()
        {
            ulong val = BitConverter.ToUInt64(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(ulong));
            return val;
        }
        public float ReadFloat()
        {
            float val = BitConverter.ToSingle(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(float));
            return val;
        }
        public double ReadDouble()
        {
            double val = BitConverter.ToDouble(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(double));
            return val;
        }

        public bool ReadBool()
        {
            bool val = BitConverter.ToBoolean(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(bool));
            return val;
        }

        public char ReadChar()
        {
            char val = BitConverter.ToChar(_buffer, _curIdx);
            _curIdx += CheckReadPossible(sizeof(char));
            return val;
        }

        public string ReadString()
        {
            int size = ReadInt32();
            CheckReadPossible(size);
            string val = Encoding.Default.GetString(_buffer, _curIdx, size);
            _curIdx += size;
            return val;
        }

        public T ReadStruct<T>() where T: IPacketSerializable, new()
        {
            T val = new T();
            val.ReadPacket(this);
            return val;
        }

        public T ReadSerializable<T>() where T : IPacketSerializable, new()
        {
            T val = new T();
            val.ReadPacket(this);
            return val;
        }

        public byte[] ReadBytes(int len)
        {
            CheckReadPossible(len);
            byte[] val = new byte[len];
            Buffer.BlockCopy(_buffer, _curIdx, val, 0, len);
            _curIdx += len;
            return val;
        }
        #endregion
    }

    public class OutPacket : InOutPacket
    {
        public OutPacket()
        {
            Clear();
        }

        public override void Clear()
        {
            _packetHeader = -1;
            _packetSize = -1;
            _curIdx = HeaderSize;
            _endIdx = HeaderSize;
        }

        public void SetPacketId(int packetId)
        {
            byte[] temp = BitConverter.GetBytes(packetId);
            Buffer.BlockCopy(temp, 0, _buffer, 0, sizeof(int));
        }
        //public void SetPacketId(PacketId packetId)
        //{
        //    int id = (int)packetId;
        //    SetPacketId(id);
        //}
        public void SetPacketSize()
        {
            _endIdx = _curIdx;
            byte[] temp = BitConverter.GetBytes(_endIdx);
            Buffer.BlockCopy(temp, 0, _buffer, 4, sizeof(int));
        }

        public bool WritePacketToBuffer(PacketBuffer buff)
        {
            while (true)
            {
                if (buff.LeftBufferSize >= Length)
                    break;

                buff.BufferExpend();
            }

            Buffer.BlockCopy(_buffer, 0, buff.GetBuffer, buff.BufferLength, Length);
            buff.AddBufferLength(Length);
            return true;
        }

        public byte[] GetBuffer
        {
            get { return _buffer; }
        }

        public void Copy(out OutPacket oPacket)
        {
            oPacket = PacketPool.Get.GetOutPacket();
            oPacket.MakeFromBuffer(_buffer, _curIdx);
        }

        public void WriteAt(int offset, byte[] data)
        {
            if (offset >= _curIdx)
                return;

            Buffer.BlockCopy(data, 0, _buffer, offset, data.Length);
        }

        #region data to byte array
        public OutPacket WritePacket(int val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(int));
            _curIdx += sizeof(int);
            return this;
        }

        public OutPacket WritePacket(uint val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(uint));
            _curIdx += sizeof(uint);
            return this;
        }

        public OutPacket WritePacket(short val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(short));
            _curIdx += sizeof(short);
            return this;
        }

        public OutPacket WritePacket(ushort val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(ushort));
            _curIdx += sizeof(ushort);
            return this;
        }

        public OutPacket WritePacket(long val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(long));
            _curIdx += sizeof(long);
            return this;
        }

        public OutPacket WritePacket(ulong val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(ulong));
            _curIdx += sizeof(ulong);
            return this;
        }

        public OutPacket WritePacket(float val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(float));
            _curIdx += sizeof(float);
            return this;
        }

        public OutPacket WritePacket(double val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(double));
            _curIdx += sizeof(double);
            return this;
        }

        public OutPacket WritePacket(char val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(char));
            _curIdx += sizeof(char);
            return this;
        }
        public OutPacket WritePacket(byte val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, sizeof(byte));
            _curIdx += sizeof(byte);
            return this;
        }

        public OutPacket WritePacket(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                WritePacket(0);
                return this;
            }

            byte[] temp = Encoding.UTF8.GetBytes(val);
            while (true)
            {
                if (temp.Length < Capacity)
                    break;

                ExpendPacket();
            }
            
            WritePacket(temp.Length);
            Buffer.BlockCopy(temp, 0, _buffer, _curIdx, temp.Length);
            _curIdx += temp.Length;
            return this;
        }

        public OutPacket WritePacket(IPacketSerializable data)
        {
            data.WritePacket(this);
            return this;
        }

        public OutPacket WritePacket(byte[] val)
        {
            int len = val.Length;
            WritePacket(len);
            Buffer.BlockCopy(val, 0, _buffer, _curIdx, len);
            _curIdx += len;
            return this;
        }

        public OutPacket WritePacket(InPacket iPacket)
        {
            // iPacket.Length;
            Buffer.BlockCopy(iPacket._buffer, iPacket.HeaderSize, _buffer, HeaderSize, iPacket.Length);
            _curIdx = iPacket.Length;
            return this;
        }
        
        #endregion
    }


    public class PacketPool
    {
        private PacketPool() {}

        private static PacketPool _PacketPool = null;
        public static PacketPool Get {
            get 
            {
                return _PacketPool;
            }
        }
        public static void Init()
        {
            if (_PacketPool == null)
            {
                _PacketPool = new PacketPool();
                _PacketPool.InitPacketPool();
            }
        }

        protected ConcurrentQueue<InPacket> _inPacketQueue = null;

        protected ConcurrentQueue<OutPacket> _outPacketQueue = null;

        public bool InitPacketPool()
        {
            if (_inPacketQueue == null)
                _inPacketQueue = new ConcurrentQueue<InPacket>();
            if (_outPacketQueue == null)
                _outPacketQueue = new ConcurrentQueue<OutPacket>();

            return true;
        }

        public InPacket GetInPacket()
        {
            InPacket iPacket;
            if (!_inPacketQueue.TryDequeue(out iPacket))
            {
                return new InPacket();
            }
            iPacket.Clear();
            return iPacket;
        }
        public OutPacket GetOutPacket()
        {
            OutPacket oPacket;
            if (!_outPacketQueue.TryDequeue(out oPacket))
            {
                return new OutPacket();
            }
            oPacket.Clear();
            return oPacket;
        }
        public void PutInPacket(InPacket iPacket)
        {
            if (iPacket == null) return;
            _inPacketQueue.Enqueue(iPacket);
        }
        public void PutOutPacket(OutPacket oPacket)
        {
            if (oPacket == null) return;
            _outPacketQueue.Enqueue(oPacket);
        }
    }

    public class PacketHelper
    {
        public static OutPacket MakeOut(int packetId)
        {
            OutPacket oPacket = PacketPool.Get.GetOutPacket();
            oPacket.SetPacketId(packetId);
            return oPacket;
        }
    }


    public class InvalidPacketException : Exception
    {
        public InvalidPacketException()
        {

        }
        public InvalidPacketException(string message)
            : base(message)
        {
        }
        public InvalidPacketException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}