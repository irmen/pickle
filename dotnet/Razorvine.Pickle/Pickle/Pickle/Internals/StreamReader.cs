﻿/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System;
using System.IO;

namespace Razorvine.Pickle
{
    internal struct StreamReader : IInputReader
    {
        private readonly Stream input;
        private byte[] buffer;

        public StreamReader(Stream input)
        {
            this.input = input;
            buffer = new byte[sizeof(long)]; // at least large enough for any primitive being deserialized;
        }

        public byte ReadByte() => PickleUtils.readbyte(input);

        public ReadOnlySpan<byte> ReadBytes(int bytesCount)
        {
            EnsureByteBufferLength(bytesCount);

            PickleUtils.readbytes_into(input, buffer, 0, bytesCount);

            return new ReadOnlySpan<byte>(buffer, 0, bytesCount);
        }

        public string ReadLine(bool includeLF = false) => PickleUtils.readline(input, includeLF);

        public ReadOnlySpan<byte> ReadLineBytes(bool includeLF = false)
        {
            int length = PickleUtils.readline_into(input, ref buffer, includeLF);

            return new ReadOnlySpan<byte>(buffer, 0, length);
        }

        public void Skip(int bytesCount)
        {
            if (input.CanSeek)
            {
                input.Seek(bytesCount, SeekOrigin.Current);
            }
            else
            {
                EnsureByteBufferLength(bytesCount);
                input.Read(buffer, 0, bytesCount);
            }
        }

        private void EnsureByteBufferLength(int bytesCount)
        {
            if (bytesCount > buffer.Length)
            {
                Array.Resize(ref buffer, Math.Max(bytesCount, buffer.Length * 2));
            }
        }
    }
}