namespace InventorySystem;

public class Dog : Animal
{
    public Dog()
    {
    }

    public Dog(String name) : base(name)
    {
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} is barking!!");
    }
}