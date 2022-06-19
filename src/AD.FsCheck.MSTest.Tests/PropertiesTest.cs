namespace AD.FsCheck.MSTest.Tests;

[TestClass]
public class PropertiesTest1
{
    [Property]
    public void Default(int x)
    {
    }
}

[TestClass]
public class PropertiesTest2
{
    [Property(MaxNbOfTest = 10)]
    public void Prop(int x)
    {
    }
}

[Properties(MaxNbOfTest = 20)]
public class PropertiesTest3
{
    [Property]
    public void ClassOnly(int x)
    {
    }


    [Property(MaxNbOfTest = 30)]
    public void ClassAndPropOnly(int x)
    {
    }
}
