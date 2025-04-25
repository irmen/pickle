/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using Xunit;
using Razorvine.Pickle;
using Razorvine.Pickle.Objects;
// ReSharper disable CheckNamespace

namespace PickleTests
{

/// <summary>
/// Unit tests for the unpickler of the special array construction
/// (Python3's array_reconstructor.)
/// </summary>
public class ArrayConstructorTest {

    [Fact]
	public void TestInvalidMachineTypes()
	{
		var ac=new ArrayConstructor();
		try {
			ac.construct('b', -1, new byte[]{0});
			Assert.Fail("expected pickleexception");
		} catch (PickleException) {
			//ok
		}

		try {
			ac.construct('b', 0, new byte[]{0});
			Assert.Fail("expected pickleexception");
		} catch (PickleException) {
			//ok
		}

		try {
			ac.construct('?', 0, new byte[]{0});
			Assert.Fail("expected pickleexception");
		} catch (PickleException) {
			//ok
		}
		
		try {
			ac.construct('b', 22, new byte[]{0});
			Assert.Fail("expected pickleexception");
		} catch (PickleException) {
			//ok
		}

		try {
			ac.construct('d', 16, new byte[]{0});
			Assert.Fail("expected pickleexception");
		} catch (PickleException) {
			//ok
		}
	}

	[Fact]
	public void TestChars()
	{
		var ac=new ArrayConstructor();
		const char eurochar = (char)0x20ac;
		
		// c/u
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('c', 18, new byte[]{65,0,0xac,0x20}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('u', 18, new byte[]{65,0,0xac,0x20}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('c', 19, new byte[]{0,65,0x20,0xac}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('u', 19, new byte[]{0,65,0x20,0xac}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('c', 20, new byte[]{65,0,0,0,0xac,0x20,0,0}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('u', 20, new byte[]{65,0,0,0,0xac,0x20,0,0}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('c', 21, new byte[]{0,0,0,65,0,0,0x20,0xac}));
		Assert.Equal(new []{'A',eurochar}, (char[])ac.construct('u', 21, new byte[]{0,0,0,65,0,0,0x20,0xac}));
		try {
		    ac.construct('u', 21, new byte[]{0,1,0,65}); // out of range codepoint
		    Assert.Fail("expected error");
		} catch (PickleException) {
		    // ok
		}
		
		// b/B
		Assert.Equal(new sbyte[]{1,2,3,4,-1,-2,-3,-4}, (sbyte[])ac.construct('b', 1, new byte[]{1,2,3,4,0xff,0xfe,0xfd,0xfc}));
		Assert.Equal(new byte[]{1,2,3,4,0xff,0xfe,0xfd,0xfc}, (byte[])ac.construct('B', 0, new byte[]{1,2,3,4,0xff,0xfe,0xfd,0xfc}));
	}
	
	[Fact]
	public void TestInts()
	{
		var ac=new ArrayConstructor();

		//h
		Assert.Equal((short)-0x7f01, ((short[])ac.construct('h', 5, new byte[]{0x80,0xff}))[0]);
		Assert.Equal((short)0x7fff, ((short[])ac.construct('h', 5, new byte[]{0x7f,0xff}))[0]);
		Assert.Equal((short)-1, ((short[])ac.construct('h', 5, new byte[]{0xff,0xff}))[0]);
		Assert.Equal((short)-1, ((short[])ac.construct('h', 4, new byte[]{0xff,0xff}))[0]);
		Assert.Equal(new short[]{0x1234,0x5678}, (short[])ac.construct('h', 5, new byte[]{0x12,0x34,0x56,0x78}));
		Assert.Equal(new short[]{0x3412,0x7856}, (short[])ac.construct('h', 4, new byte[]{0x12,0x34,0x56,0x78}));

		//H
		Assert.Equal((ushort)0x80ff, ((ushort[])ac.construct('H', 3, new byte[]{0x80,0xff}))[0]);
		Assert.Equal((ushort)0x7fff, ((ushort[])ac.construct('H', 3, new byte[]{0x7f,0xff}))[0]);
		Assert.Equal((ushort)0xffff, ((ushort[])ac.construct('H', 3, new byte[]{0xff,0xff}))[0]);
		Assert.Equal((ushort)0xffff, ((ushort[])ac.construct('H', 2, new byte[]{0xff,0xff}))[0]);
		Assert.Equal(new ushort[]{0x1234,0x5678}, (ushort[])ac.construct('H', 3, new byte[]{0x12,0x34,0x56,0x78}));
		Assert.Equal(new ushort[]{0x3412,0x7856}, (ushort[])ac.construct('H', 2, new byte[]{0x12,0x34,0x56,0x78}));

		//i
		Assert.Equal(-0x7fffff01, ((int[])ac.construct('i', 9, new byte[]{0x80,0x00,0x00,0xff}))[0]);
		Assert.Equal(0x7f0000ff, ((int[])ac.construct('i', 9, new byte[]{0x7f,0x00,0x00,0xff}))[0]);
		Assert.Equal(-0xfffff0f, ((int[])ac.construct('i', 9, new byte[]{0xf0,0x00,0x00,0xf1}))[0]);
		Assert.Equal(-2, ((int[])ac.construct('i', 8, new byte[]{0xfe,0xff,0xff,0xff}))[0]);
		Assert.Equal(new []{0x11223344,0x55667788}, (int[])ac.construct('i', 9, new byte[]{0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88}));
		Assert.Equal(new []{0x44332211,-0x778899ab}, (int[])ac.construct('i', 8, new byte[]{0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88}));

		//l-4bytes
		Assert.Equal(-0x7fffff01, ((int[])ac.construct('l', 9, new byte[]{0x80,0x00,0x00,0xff}))[0]);
		Assert.Equal(0x7f0000ff, ((int[])ac.construct('l', 9, new byte[]{0x7f,0x00,0x00,0xff}))[0]);
		Assert.Equal(-0x0fffff0f, ((int[])ac.construct('l', 9, new byte[]{0xf0,0x00,0x00,0xf1}))[0]);
		Assert.Equal(-2, ((int[])ac.construct('l', 8, new byte[]{0xfe,0xff,0xff,0xff}))[0]);
		Assert.Equal(new []{0x11223344,0x55667788}, (int[])ac.construct('l', 9, new byte[]{0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88}));
		Assert.Equal(new []{0x44332211,-0x778899ab}, (int[])ac.construct('l', 8, new byte[]{0x11,0x22,0x33,0x44,0x55,0x66,0x77,0x88}));
		//l-8bytes
		Assert.Equal(0x3400000000000012L, ((long[])ac.construct('l', 12, new byte[]{0x12,0x00,0x00,0x00,0x00,0x00,0x00,0x34}))[0]);
		Assert.Equal(0x3400009009000012L, ((long[])ac.construct('l', 12, new byte[]{0x12,0x00,0x00,0x09,0x90,0x00,0x00,0x34}))[0]);
		Assert.Equal(0x1200000000000034L, ((long[])ac.construct('l', 13, new byte[]{0x12,0x00,0x00,0x00,0x00,0x00,0x00,0x34}))[0]);
		Assert.Equal(0x1200000990000034L, ((long[])ac.construct('l', 13, new byte[]{0x12,0x00,0x00,0x09,0x90,0x00,0x00,0x34}))[0]);

		Assert.Equal(0x7fffffffffffffffL, ((long[])ac.construct('l', 13, new byte[]{0x7f,0xff,0xff,0xff,0xff,0xff,0xff,0xff}))[0]);
		Assert.Equal(0x7fffffffffffffffL, ((long[])ac.construct('l', 12, new byte[]{0xff,0xff,0xff,0xff,0xff,0xff,0xff,0x7f}))[0]);

		Assert.Equal(-2L, ((long[])ac.construct('l', 12, new byte[]{0xfe,0xff,0xff,0xff,0xff,0xff,0xff,0xff}))[0]);
		Assert.Equal(-2L, ((long[])ac.construct('l', 13, new byte[]{0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xfe}))[0]);
		Assert.Equal(new long[]{1,2}, (long[])ac.construct('l', 13, new byte[]{0,0,0,0,0,0,0,1, 0,0,0,0,0,0,0,2}));
		Assert.Equal(new long[]{1,2}, (long[])ac.construct('l', 12, new byte[]{1,0,0,0,0,0,0,0, 2,0,0,0,0,0,0,0}));

		//I 
		Assert.Equal(0x001000000u, ((uint[])ac.construct('I', 6, new byte[]{0,0,0,0x01}))[0]);
		Assert.Equal(0x088000000u, ((uint[])ac.construct('I', 6, new byte[]{0,0,0,0x88}))[0]);
		Assert.Equal(0x000000001u, ((uint[])ac.construct('I', 7, new byte[]{0,0,0,0x01}))[0]);
		Assert.Equal(0x000000088u, ((uint[])ac.construct('I', 7, new byte[]{0,0,0,0x88}))[0]);
		Assert.Equal(0x099000088u, ((uint[])ac.construct('I', 7, new byte[]{0x99,0,0,0x88}))[0]);

		//L-4 bytes
		Assert.Equal(0x22000011U, ((uint[])ac.construct('L', 6, new byte[]{0x11,0,0,0x22}))[0]);
		Assert.Equal(0x11000022U, ((uint[])ac.construct('L', 7, new byte[]{0x11,0,0,0x22}))[0]);
		Assert.Equal(0xfffffffeU, ((uint[])ac.construct('L', 6, new byte[]{0xfe,0xff,0xff,0xff}))[0]);
		Assert.Equal(0xfffffffeU, ((uint[])ac.construct('L', 7, new byte[]{0xff,0xff,0xff,0xfe}))[0]);
    	
    	//L-8 bytes
		Assert.Equal(0x4400003322000011UL, ((ulong[])ac.construct('L', 10, new byte[]{0x11,0,0,0x22,0x33,0,0,0x44}))[0]);
		Assert.Equal(0x1100002233000044UL, ((ulong[])ac.construct('L', 11, new byte[]{0x11,0,0,0x22,0x33,0,0,0x44}))[0]);
		Assert.Equal(0xfffffffffffffffeUL, ((ulong[])ac.construct('L', 10, new byte[]{0xfe,0xff,0xff,0xff,0xff,0xff,0xff,0xff}))[0]);
		Assert.Equal(0xfffffffffffffffeUL, ((ulong[])ac.construct('L', 11, new byte[]{0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xfe}))[0]);
	}
	
	[Fact]
	public void TestFloats()
	{
		// f/d
		var ac=new ArrayConstructor();
		Assert.Equal(16711938.0f,
				((float[])ac.construct('f', 15, new byte[]{0x4b,0x7f,0x01,0x02}))[0] );
		Assert.Equal(float.PositiveInfinity,
				((float[])ac.construct('f', 15, new byte[]{0x7f,0x80,0x00,0x00}))[0]);
		Assert.Equal(float.NegativeInfinity,
				((float[])ac.construct('f', 15, new byte[]{0xff,0x80,0x00,0x00}))[0]);
		Assert.Equal(-0.0f,
				((float[])ac.construct('f', 15, new byte[]{0x80,0x00,0x00,0x00}))[0]);
		
		Assert.Equal(16711938.0f,
				((float[])ac.construct('f', 14, new byte[]{0x02,0x01,0x7f,0x4b}))[0]);
		Assert.Equal(float.PositiveInfinity,
				((float[])ac.construct('f', 14, new byte[]{0x00,0x00,0x80,0x7f}))[0]);
		Assert.Equal(float.NegativeInfinity,
				((float[])ac.construct('f', 14, new byte[]{0x00,0x00,0x80,0xff}))[0]);
		Assert.Equal(-0.0f,
				((float[])ac.construct('f', 14, new byte[]{0x00,0x00,0x00,0x80}))[0]);

		Assert.Equal(9006104071832581.0d,
				((double[])ac.construct('d', 17, new byte[]{0x43,0x3f,0xff,0x01,0x02,0x03,0x04,0x05}))[0]);
		Assert.Equal(double.PositiveInfinity,
				((double[])ac.construct('d', 17, new byte[]{0x7f,0xf0,0x00,0x00,0x00,0x00,0x00,0x00}))[0]);
		Assert.Equal(double.NegativeInfinity,
				((double[])ac.construct('d', 17, new byte[]{0xff,0xf0,0x00,0x00,0x00,0x00,0x00,0x00}))[0]);
		Assert.Equal(-0.0d,
				((double[])ac.construct('d', 17, new byte[]{0x80,0x00,0x00,0x00,0x00,0x00,0x00,0x00}))[0]);
		
		Assert.Equal(9006104071832581.0d,
				((double[])ac.construct('d', 16, new byte[]{0x05,0x04,0x03,0x02,0x01,0xff,0x3f,0x43}))[0]);
		Assert.Equal(double.PositiveInfinity,
				((double[])ac.construct('d', 16, new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0xf0,0x7f}))[0]);
		Assert.Equal(double.NegativeInfinity,
				((double[])ac.construct('d', 16, new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0xf0,0xff}))[0]);
		Assert.Equal(-0.0d,
				((double[])ac.construct('d', 16, new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x80}))[0]);

	
		// check if multiple values in an array work
		Assert.Equal(new []{1.1f, 2.2f}, (float[])  ac.construct('f', 15, new byte[]{0x3f,0x8c,0xcc,0xcd, 0x40,0x0c,0xcc,0xcd}));
		Assert.Equal(new []{1.1f, 2.2f}, (float[])  ac.construct('f', 14, new byte[]{0xcd,0xcc,0x8c,0x3f, 0xcd,0xcc,0x0c,0x40}));
		Assert.Equal(new []{1.1d, 2.2d}, (double[]) ac.construct('d', 17, new byte[]{0x3f,0xf1,0x99,0x99,0x99,0x99,0x99,0x9a, 0x40,0x01,0x99,0x99,0x99,0x99,0x99,0x9a}));
		Assert.Equal(new []{1.1d, 2.2d}, (double[]) ac.construct('d', 16, new byte[]{0x9a,0x99,0x99,0x99,0x99,0x99,0xf1,0x3f, 0x9a,0x99,0x99,0x99,0x99,0x99,0x01,0x40}));
	}
}

}
