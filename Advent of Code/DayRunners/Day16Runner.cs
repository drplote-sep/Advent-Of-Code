using System;
using System.Collections.Generic;
using System.Linq;
using Advent_of_Code.DataSources;

namespace Advent_of_Code.DayRunners
{
    public static class ListExtensions
    {
        public static List<T> TakeAndRemove<T>(this List<T> list, int num)
        {
            var e = list.Take(num).ToList();
            list.RemoveRange(0, num);
            return e;
        }
    }

    public class TransmissionPacket
    {
        public int PacketVersion { get; }
        public int PacketType { get; }
        
        public int? LengthTypeId { get; set; }
        public long LiteralValue { get; set; }
        public int? LengthValue { get; set; }
        
        public List<TransmissionPacket> SubPackets { get; } = new List<TransmissionPacket>();

        public TransmissionPacket(int packetVersion, int packetType)
        {
            PacketVersion = packetVersion;
            PacketType = packetType;
        }

        public int GetPacketVersionSum()
        {
            return PacketVersion + SubPackets.Sum(p => p.GetPacketVersionSum());
        }

        public long GetValue()
        {
            switch (PacketType)
            {
                case 0:
                    return SubPackets.Sum(p => p.GetValue());
                case 1:
                    return SubPackets.Select(p => p.GetValue()).Aggregate((a, x) => a * x);
                case 2:
                    return SubPackets.Min(p => p.GetValue());
                case 3:
                    return SubPackets.Max(p => p.GetValue());
                case 4:
                    return LiteralValue;
                case 5:
                    if (SubPackets.Count != 2)
                    {
                        throw new InvalidOperationException("Expected exactly 2 packets for greater-than");
                    }
                    return SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0;
                case 6:
                    if (SubPackets.Count != 2)
                    {
                        throw new InvalidOperationException("Expected exactly 2 packets for greater-than");
                    }
                    return SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0;
                case 7:
                    if (SubPackets.Count != 2)
                    {
                        throw new InvalidOperationException("Expected exactly 2 packets for greater-than");
                    }
                    return SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0;
                default:
                    throw new InvalidOperationException("Unknown packet type");
            }
        }
    }
    
    public class Day16Runner : DayRunner
    {
        public string InputString { get; set; }
        public List<byte> InputBytes { get; set; }
        public List<int> InputBits { get; set; }
        
        public TransmissionPacket TopPacket { get; set; }
        
        public Day16Runner(DayData data) : base(data)
        {
        }

        protected override void SolveDay(string[] data)
        {
            ParseInput(data);
            ParsePackets();
            
            //OutputWriter.WriteResult(1, $"Literal value: {TopPacket.LiteralValue}");
            
            OutputWriter.WriteResult(1, $"Sum of version # of all packets: {TopPacket.GetPacketVersionSum()}");
            OutputWriter.WriteResult(2, $"Value of packets: {TopPacket.GetValue()}");
            
        }

        private void ParsePackets()
        {
            TopPacket = ParsePacket(InputBits.ToList());
        }

        private TransmissionPacket ParsePacket(List<int> remainingBits)
        {
            var versionBits = remainingBits.TakeAndRemove(3);
            var typeBits = remainingBits.TakeAndRemove(3);
            
            var packet = new TransmissionPacket(GetPacketVersion(versionBits), GetPacketType(typeBits));
            if (packet.PacketType == 4)
            {
                packet.LiteralValue = BitsToLiteralValue(remainingBits);
            }
            else
            {
                packet.LengthTypeId = remainingBits.TakeAndRemove(1).Single();
                switch (packet.LengthTypeId)
                {
                    case 0:
                        packet.LengthValue = BitsToDecimal(remainingBits.TakeAndRemove(15));
                        var numSubpacketBitsRead = 0;
                        while  (numSubpacketBitsRead < packet.LengthValue)
                        {
                            var numBitsRemaining = remainingBits.Count;
                            packet.SubPackets.Add(ParsePacket(remainingBits));
                            numSubpacketBitsRead += numBitsRemaining - remainingBits.Count;
                        }
                        break;
                    case 1:
                        packet.LengthValue = BitsToDecimal(remainingBits.TakeAndRemove(11));
                        for (int i = 0; i < packet.LengthValue; i++)
                        {
                            packet.SubPackets.Add(ParsePacket(remainingBits));
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Type Length ID should be 0 or 1");
                }
            }

            return packet;
        }

        private int BitsToDecimal(IEnumerable<int> bits)
        {
            return Convert.ToInt32(string.Join(string.Empty, bits), 2);
        }

        private long BitsToLiteralValue(List<int> bits)
        {
            var s = string.Empty;
            while (bits.Any())
            {
                var curBits = bits.TakeAndRemove(5);
                s += string.Join("", curBits.Skip(1));
                if (curBits[0] == 0)
                {
                    return Convert.ToInt64(s, 2);
                }
            }

            throw new InvalidOperationException("Ran out of bits while decoding literal value but never saw 0 to indicate end byte");
        }

        private int GetPacketVersion(IEnumerable<int> bits)
        {
            return BitsToInt(bits);
        }

        public int GetPacketType(IEnumerable<int> bits)
        {
            return BitsToInt(bits);
        }

        public int BitsToInt(IEnumerable<int> bits)
        {
            return Convert.ToInt32(string.Join("", bits), 2);
        }
        

        private void ParseInput(string[] data)
        {
            InputString = data.First();
            InputBytes = InputString.Select(HexToByte).ToList();
            InputBits = new List<int>(InputBytes.SelectMany(b => ToBitArray(b)));
        }

        private IEnumerable<int> ToBitArray(byte b)
        {
            var s = Convert.ToString(b, 2);
            while (s.Length < 4)
            {
                s = s.Insert(0, "0");
            }

            return s.Select(c => c == '1' ? 1 : 0);
        }

        private byte HexToByte(string s)
        {
            return Convert.ToByte(s, 16);
        }

        private byte HexToByte(char hexChar)
        {
            return HexToByte(hexChar.ToString());
        }
    }
}