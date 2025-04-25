/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System;
using System.Collections;
using System.Text;

namespace Razorvine.Pickle.Objects
{

/// <summary>
/// Creates byte arrays (byte[]). 
/// </summary>
public class ByteArrayConstructor : IObjectConstructor {

	public object construct(object[] args) {
		// args for bytearray constructor: [ String string, String encoding ]
		// args for bytearray constructor (from python3 bytes): [ ArrayList ] or just [byte[]] (when it uses BINBYTES opcode)
		// or, zero arguments: empty bytearray.
		if (args.Length>2)
			throw new PickleException("invalid pickle data for bytearray; expected 0, 1 or 2 args, got "+args.Length);

		switch (args.Length)
		{
			case 0:
				return new byte[]{};
			case 1 when args[0] is byte[]:
				return args[0];
			case 1:
			{
				var values=(ArrayList) args[0];
				var data=new byte[values.Count];
				for(int i=0; i<data.Length; ++i) {
					data[i] = Convert.ToByte(values[i]);
				}
				return data;
			}
			default:
			{
				// This thing is fooling around with byte<>string mappings using an encoding.
				// I think that is fishy... but for now it seems what Python itself is also doing...
				string data = (string) args[0];
				string encoding = (string) args[1];
				if (encoding.StartsWith("latin-"))
					encoding = "ISO-8859-" + encoding.Substring(6);
				return Encoding.GetEncoding(encoding).GetBytes(data);
			}
		}
	}
}

}
