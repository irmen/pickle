# Pickle - Java and .NET library for Python's pickle serialization protocol

[![Maven Central](https://img.shields.io/maven-central/v/net.razorvine/pickle.svg)](http://search.maven.org/#search|ga|1|g%3A%22net.razorvine%22%20AND%20a%3A%22pickle%22)
[![NuGet](https://img.shields.io/nuget/v/Razorvine.Pickle.svg)](https://www.nuget.org/packages/Razorvine.Pickle/)



Pickle is written by Irmen de Jong (irmen@razorvine.net).
This software is distributed under the terms written in the file `LICENSE`.


## The pickle serialization protocol

This is a feature complete pickle protocol implementation.
You can read and write pickle files.
Pickle is Python's serialization protocol.


* **Java**: from Maven, group id ``net.razorvine`` artifact id ``pickle``.
* **.NET**: nuget Razorvine.Pickle; https://www.nuget.org/packages/Razorvine.Pickle/



Pickle protocol version support: reading: 0,1,2,3,4,5;  writing: 2.
We can read all pickle protocol versions (0 to 5, so this includes
the latest additions made in Python 3.8 related to out-of-band buffers).
We always writes pickles in protocol version 2. There are no plans on 
including protocol version 1 support. Protocols 3 and 4 contain some nice new
features which may eventually be utilized (protocol 5 is quite obscure),
but for now, only version 2 is used.

## Size limitations

Unlike Python where the length of strings and (byte)arrays is only limited by the available memory,
Java and .NET do have an arbitrary maximum object size.
The maximum length of strings and byte arrays of both platforms is limited to 2 gigabytes (2^31 - 1).
This is not a Pickle library limitation, this is a limitation of the underlying platform.
If an object in your pickle exceeds this limit the code will crash with something like an
``NegativeArraySizeException``, ``OverflowException`` or perhaps an out of memory error of some sort.
You should make sure in your own code that the size of the pickled objects does not exceed 2 gigabyte.


## Type Mapping

### Python to Java (unpickling)

The Unpickler simply returns an Object. Because Java is a statically typed
language you will have to cast that to the appropriate type. Refer to this
table to see what you can expect to receive.

PYTHON             |JAVA
-------------------|----------------------
None               | null
bool               | boolean
int                | int
long               | long or BigInteger  (depending on size)
string             | String
unicode            | String
complex            | net.razorvine.pickle.objects.ComplexNumber
datetime.date      | java.util.Calendar
datetime.datetime  | java.util.Calendar
datetime.time      | net.razorvine.pickle.objects.Time
datetime.timedelta | net.razorvine.pickle.objects.TimeDelta
float              | double   (float isn't used)
array.array        | array of appropriate primitive type (char, int, short, long, float, double)
list               | java.util.List<Object>
tuple              | Object[]
set                | java.util.Set
dict               | java.util.Map
bytes              | byte[]
bytearray          | byte[]
decimal            | BigDecimal (except NaN which is mapped to Double.NaN)
custom class       | Map<String, Object>  (dict with class attributes including its name in "__class__")
Pyro4.core.URI     | net.razorvine.pyro.PyroURI
Pyro4.core.Proxy   | net.razorvine.pyro.PyroProxy
Pyro4.errors.*     | net.razorvine.pyro.PyroException
Pyro4.utils.flame.FlameBuiltin    | net.razorvine.pyro.FlameBuiltin 
Pyro4.utils.flame.FlameModule     | net.razorvine.pyro.FlameModule 
Pyro4.utils.flame.RemoteInteractiveConsole   | net.razorvine.pyro.FlameRemoteConsole 


### Java to Python  (pickling)

JAVA               | PYTHON
-------------------|-----------------------
null               | None
boolean            | bool
byte               | int
char               | str/unicode (length 1)
String             | str/unicode
double             | float
float              | float
int                | int
short              | int
BigDecimal         | decimal
BigInteger         | long
any array          | array if elements are primitive type (else tuple)
Object[]           | tuple (cannot contain self-references)
byte[]             | bytearray
java.util.Date     | datetime.datetime
java.util.Calendar | datetime.datetime
java.sql.Date      | datetime.date
java.sql.Time      | datetime.time
java.sql.Timestamp | datetime.datetime
Enum               | the enum value as string
java.util.Set      | set
Map, Hashtable     | dict
Vector, Collection | list
Serializable       | treated as a JavaBean, see below.
JavaBean           | dict of the bean's public properties + ``__class__`` for the bean's type.
net.razorvine.pyro.PyroURI     | Pyro4.core.URI
net.razorvine.pyro.PyroProxy   | cannot be pickled.

### Python to .NET (unpickling)

The unpickler simply returns an object. In the case of C#, that is a statically typed
language so you will have to cast that to the appropriate type. Refer to this
table to see what you can expect to receive. Tip: you can use the 'dynamic' type 
in some places to avoid excessive type casting.


PYTHON              | .NET
--------------------|-------------
None                | null
bool                | bool
int                 | int
long                | long (c# doesn't have BigInteger so there's a limit on the size)
string              | string
unicode             | string
complex             | Razorvine.Pickle.Objects.ComplexNumber
datetime.date       | DateTime
datetime.datetime   | DateTime
datetime.time       | TimeSpan
datetime.timedelta  | TimeSpan
float               | double
array.array         | array (all kinds of element types supported)
list                | ArrayList (of objects)
tuple               | object[]
set                 | HashSet<object>
dict                | Hashtable (key=object, value=object)
bytes               | ubyte[]
bytearray           | ubyte[]
decimal             | decimal (except NaN which is mapped to double.NaN)
custom class        | IDictionary<string, object>  (dict with class attributes including its name in "__class__")
Pyro4.core.URI      | Razorvine.Pyro.PyroURI
Pyro4.core.Proxy    | Razorvine.Pyro.PyroProxy
Pyro4.errors.*      | Razorvine.Pyro.PyroException
Pyro4.utils.flame.FlameBuiltin    | Razorvine.Pyro.FlameBuiltin 
Pyro4.utils.flame.FlameModule     | Razorvine.Pyro.FlameModule 
Pyro4.utils.flame.RemoteInteractiveConsole   | Razorvine.Pyro.FlameRemoteConsole 


### .NET to Python (pickling)


.NET                | PYTHON
--------------------|---------------
null                | None
boolean             | bool
byte                | byte
sbyte               | int
char                | str/unicode (length 1)
string              | str/unicode
double              | float
float               | float
int/short/sbyte     | int
uint/ushort/byte    | int
decimal             | decimal
byte[]              | bytearray
primitivetype[]     | array
object[]            | tuple  (cannot contain self-references)
DateTime            | datetime.datetime
TimeSpan            | datetime.timedelta
Enum                | just the enum value as string
HashSet             | set
Map, Hashtable      | dict
Collection          | list
Enumerable          | list
object with public properties      | dictionary of those properties + __class__
anonymous class type        | dictonary of the public properties
Razorvine.Pyro.PyroURI      | Pyro4.core.URI
Razorvine.Pyro.PyroProxy    | cannot be pickled.

