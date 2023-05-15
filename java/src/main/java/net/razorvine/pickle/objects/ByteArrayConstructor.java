package net.razorvine.pickle.objects;

import net.razorvine.pickle.IObjectConstructor;
import net.razorvine.pickle.PickleException;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;

/**
 * Creates byte arrays (byte[]).
 *
 * @author Irmen de Jong (irmen@razorvine.net)
 */
public class ByteArrayConstructor implements IObjectConstructor {

	public Object construct(Object[] args) throws PickleException {
		// args for bytearray constructor: [ String string, String encoding ]
		// args for bytearray constructor (from python3 bytes): [ ArrayList<Number> ] or just [byte[]] (when it uses BINBYTES opcode)
		// or, zero arguments: empty bytearray.
		if (args.length>2)
			throw new PickleException("invalid pickle data for bytearray; expected 0, 1 or 2 args, got "+args.length);

		if(args.length==0)
			return new byte[0];

		if(args.length==1) {
			if(args[0] instanceof byte[]) {
				return args[0];
			}
			@SuppressWarnings("unchecked")
			ArrayList<Number>values=(ArrayList<Number>)args[0];
			byte[] data=new byte[values.size()];
			for(int i=0; i<data.length; ++i) {
				data[i] = values.get(i).byteValue();
			}
			return data;
		} else {
			String data = (String) args[0];
			String encoding = (String) args[1];
			if (encoding.startsWith("latin-"))
				encoding = "ISO-8859-" + encoding.substring(6);
			try {
				return data.getBytes(encoding);
			} catch (UnsupportedEncodingException e) {
				throw new PickleException("error creating bytearray: " + e);
			}
		}
	}
}
