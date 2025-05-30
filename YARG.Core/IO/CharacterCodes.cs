﻿using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using YARG.Core.Extensions;

namespace YARG.Core.IO
{
    /// <summary>
    /// A four-byte identifier ("four-character code") used to identify data formats.
    /// </summary>
    /// <remarks>
    /// These are read and written in big-endian, so that the characters used are
    /// human-readable in a hex editor, for example.
    /// </remarks>
    public readonly struct FourCC
    {
        private readonly uint _code;

        public FourCC(char a, char b, char c, char d)
        {
            _code = ((uint)(byte) d << 24) | ((uint)(byte) c << 16) | ((uint)(byte) b << 8) | a;
        }

        public FourCC(ReadOnlySpan<byte> data)
        {
            _code = BinaryPrimitives.ReadUInt32LittleEndian(data);
        }

        public FourCC(Stream stream)
        {
            _code = stream.Read<uint>(Endianness.Little);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.BaseStream.Write(_code, Endianness.Little);
        }

        public bool Matches(Stream stream)
        {
            return stream.Read<uint>(Endianness.Little) == _code;
        }

        public bool Matches(ref FixedArrayStream stream)
        {
            return stream.Read<uint>(Endianness.Little) == _code;
        }

        public bool Matches(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(data) == _code;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(FourCC left, FourCC right) => left._code == right._code;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(FourCC left, FourCC right) => left._code != right._code;

        public bool Equals(FourCC other) => this == other;
        public override bool Equals(object obj) => obj is FourCC cc && Equals(cc);
        public override int GetHashCode() => _code.GetHashCode();

        public override string ToString()
        {
            char d = (char) ((_code >> 24) & 0xFF);
            char c = (char) ((_code >> 16) & 0xFF);
            char b = (char) ((_code >> 8) & 0xFF);
            char a = (char) (_code & 0xFF);
            return $"{a}{b}{c}{d}";
        }
    }

    /// <summary>
    /// An eight-byte identifier ("eight-character code") used to identify data formats.
    /// </summary>
    /// <remarks>
    /// These are read and written in big-endian, so that the characters used are
    /// human-readable in a hex editor, for example.
    /// </remarks>
    public readonly struct EightCC
    {
        private readonly ulong _code;

        public EightCC(char a, char b, char c, char d, char e, char f, char g, char h)
        {
            _code = ((ulong) (byte) h << 56) | ((ulong) (byte) g << 48) | ((ulong) (byte) f << 40) | ((ulong) (byte) e << 32) |
                    ((ulong) (byte) d << 24) | ((ulong) (byte) c << 16) | ((ulong) (byte) b << 8)  | a;
        }

        public EightCC(ReadOnlySpan<byte> data)
        {
            _code = BinaryPrimitives.ReadUInt64LittleEndian(data);
        }

        public EightCC(Stream stream)
        {
            _code = stream.Read<ulong>(Endianness.Little);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.BaseStream.Write(_code, Endianness.Little);
        }

        public bool Matches(Stream stream)
        {
            return stream.Read<ulong>(Endianness.Little) == _code;
        }

        public bool Matches(ref FixedArrayStream stream)
        {
            return stream.Read<ulong>(Endianness.Little) == _code;
        }

        public bool Matches(ReadOnlySpan<byte> data)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(data) == _code;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(EightCC left, EightCC right) => left._code == right._code;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(EightCC left, EightCC right) => left._code != right._code;

        public bool Equals(EightCC other) => this == other;
        public override bool Equals(object obj) => obj is EightCC cc && Equals(cc);
        public override int GetHashCode() => _code.GetHashCode();

        public override string ToString()
        {
            char h = (char) ((_code >> 56) & 0xFF);
            char g = (char) ((_code >> 48) & 0xFF);
            char f = (char) ((_code >> 40) & 0xFF);
            char e = (char) ((_code >> 32) & 0xFF);
            char d = (char) ((_code >> 24) & 0xFF);
            char c = (char) ((_code >> 16) & 0xFF);
            char b = (char) ((_code >> 8) & 0xFF);
            char a = (char) (_code & 0xFF);
            return $"{a}{b}{c}{d}{e}{f}{g}{h}";
        }
    }
}