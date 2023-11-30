using NetDemo.Network.IOPacket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetDemo.Network.NetObjects
{
    #region packet buffers
    public class PacketStream : MemoryStream
    {
        public long LeftSize { get { return Capacity - EndIndex; } }

        public long EndIndex = 0;
    }
    public class RecvPacketStream : PacketStream
    {

    }
    public class PacketBuffer
    {
        //현재 버퍼 내부에서 read 작업을 수행한 마지막 위치
        protected int _curIdx = 0;
        //현재 버퍼 내부에서 유효한 데이터의 가장 마지막 위치
        //write 작업시 여기를 증가 시키자.
        protected int _endIdx = 0;
        //현재 버퍼가 담을수 있는 최대 데이터의 크기
        protected int _bufferSize = 1024 * 4;
        //private List<byte> _packetBuffer = new List<byte>();
        protected byte[] _packetBuffer = null;

        public PacketBuffer()
        {
            _packetBuffer = new byte[_bufferSize];
        }
        public int BufferLength
        {
            get { return _endIdx; }
        }
        public void AddBufferLength(int length)
        {
            _endIdx += length;
        }
        public int LeftBufferSize
        {
            get { return _bufferSize - _endIdx; }
        }
        public int LeftDataSize
        {
            get { return _endIdx - _curIdx; }
        }
        public byte[] GetBuffer
        {
            get { return _packetBuffer; }
        }
        public int CurPos
        {
            set { _curIdx = value; }
            get { return _curIdx; }
        }

        public void BufferExpend()
        {
            _bufferSize *= 2;
            byte[] _buff = new byte[_bufferSize];
            Array.Copy(_packetBuffer, _buff, BufferLength);
            _packetBuffer = _buff;
        }

        public void ClearBuffer()
        {
            _curIdx = 0;
            _endIdx = 0;
        }

        //public 
    }
    public class RecvPacketBuffer : PacketBuffer
    {
        public byte[] PacketBuffer { get { return _packetBuffer; } }
        public int HeadPos { get { return _endIdx; } }

        public void PopParseData()
        {
            int left_size = LeftDataSize;
            Buffer.BlockCopy(_packetBuffer, _curIdx, _packetBuffer, 0, left_size);
            _curIdx = 0;
            _endIdx = left_size;
        }
    }

    public class SendPacketBuffer
    {
        protected PacketBuffer _buffer;
        protected ConcurrentQueue<OutPacket> _packetQueue;

        public SendPacketBuffer()
        {
            _buffer = new PacketBuffer();
            _packetQueue = new ConcurrentQueue<OutPacket>();
        }

        public void PushPacket(OutPacket oPacket)
        {
            _packetQueue.Enqueue(oPacket);
        }

        public bool IsQueueEmpty
        {
            get { return _packetQueue.IsEmpty; }
        }
        public bool IsSendComplete
        {
            get { return _buffer.LeftDataSize == 0; }
        }
        public PacketBuffer SendBuffer
        {
            get { return _buffer; }
        }

        public void FlushPackets()
        {
            int queue_count = _packetQueue.Count;
            OutPacket oPacket;
            for (int i = 0; i < queue_count; ++i)
            {
                if (false == _packetQueue.TryDequeue(out oPacket))
                    return;

                oPacket.WritePacketToBuffer(_buffer);
                PacketPool.Get.PutOutPacket(oPacket);
            }
        }

        public void PopFrontBuffer(int size)
        {
            int popPos = _buffer.CurPos + size;
            if (popPos >= _buffer.BufferLength)
            {
                _buffer.CurPos = _buffer.BufferLength;
                return;
            }

            _buffer.CurPos = popPos;
        }

        public void ClearBuffer()
        {
            _buffer.ClearBuffer();
        }
    }
    #endregion
}