namespace AD.FsCheck.MSTest.FsTests

open System
open System.Threading.Tasks
open Microsoft.VisualStudio.TestTools.UnitTesting
open AD.FsCheck.MSTest

type Vector = { X: int; Y: int }
with static member (+) (a, b) = { X = a.X + b.X; Y = a.Y + b.Y }

module Vector =
    let plusIdentity = { X = 0; Y = 0}

[<Properties(MaxNbOfTest = 1000)>]
type VectorTest () =
    
    [<Property>]
    member _.``Plus is commutative`` (a: Vector, b) = Assert.AreEqual<_> (a + b, b + a)

    [<Property>]
    member _.``Plus is associative`` (a: Vector, b, c) = Assert.AreEqual<_> (a + b + c, a + (b + c))

    [<Property>]
    member _.``There is a plus identity element`` a = Assert.AreEqual<_> (a, a + Vector.plusIdentity)

[<TestClass>]
type VectorSerializationTest () =
    
    let serialize vector = task {
        let data = [| yield! vector.X |> BitConverter.GetBytes; yield! vector.Y |> BitConverter.GetBytes |]
        do! Task.Delay 10
        return data
    }

    let deserialize data = task {
        let vector = { X = (data, 0) |> BitConverter.ToInt32; Y = (data, sizeof<int>) |> BitConverter.ToInt32 }
        do! Task.Delay 10
        return vector
    }

    [<Property(MaxNbOfTest = 10)>]
    member _.```Serialize and deserialize`` expected : Task = task {
        let! data = expected |> serialize
        let! actual = data |> deserialize
        Assert.AreEqual<_> (expected, actual)
    }
