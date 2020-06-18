module Tests

open MessingUp

open Xunit


[<Fact>]
let ``This useless unit test should work!`` () =
    let name = "michelle"
    let actual = Say.hello name
    let expected = sprintf "Hello %s" name
    Assert.Equal(expected, actual)
