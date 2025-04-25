/* part of Pickle, by Irmen de Jong (irmen@razorvine.net) */

using System.Collections;
using Xunit;
using Razorvine.Pickle;
// ReSharper disable CheckNamespace

namespace PickleTests
{

/// <summary>
/// Unit tests for the unpickler stack object. 
/// </summary>
public class UnpickleStackTest {

	[Fact]
	public void TestPopSinceMarker() {
		var s=new UnpickleStack();
		s.add("a");
		s.add("b");
		s.add_mark();
		s.add("c");
		s.add("d");
		s.add_mark();
		s.add("e");
		s.add("f");
		var top=s.pop_all_since_marker();
		var expected = new ArrayList {"e", "f"};
		Assert.Equal(expected, top);
		Assert.Equal("d",s.pop());
		Assert.Equal("c",s.pop());
	}

	[Fact]
	public void TestAddPop() {
		var s=new UnpickleStack();
		Assert.Equal(0, s.size());
		s.add("x");
		Assert.Equal(1, s.size());
		s.add("y");
		Assert.Equal(2, s.size());
		Assert.Equal("y", s.peek());
		Assert.Equal("y", s.pop());
		Assert.Equal("x", s.peek());
		Assert.Equal("x", s.pop());
		Assert.Equal(0, s.size());
	}

	[Fact]
	public void TestClear() {
		var s=new UnpickleStack();
		s.add("x");
		s.add("y");
		Assert.Equal(2, s.size());
		s.clear();
		Assert.Equal(0, s.size());
	}

	[Fact]
	public void TestTrim() {
		var s=new UnpickleStack();
		s.add("a");
		s.add("b");
		s.add("c");
		s.add("d");
		s.add("e");
		Assert.Equal(5, s.size());
		s.trim();
		Assert.Equal(5, s.size());
	}
}

}
