namespace InventorySystem;

public class Animal
{
    public String Name { get; set; }

    public Animal()
    {
    }

    public Animal(string name)
    {
        Name = name;
    }

    public virtual void MakeSound()
    {
        Console.WriteLine($"{Name} make sound");
    }
}