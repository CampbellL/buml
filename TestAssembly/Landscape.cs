using System;
using System.Collections.Generic;

namespace TestAssembly
{
    class Landscape
    {
        List<ArchitectObject> objects;
    }


    interface IPrototype
    {
        IPrototype Clone();
    }

    abstract class ArchitectObject : IPrototype
    {
        public int height;
        public string color;

        public ArchitectObject(int height = 0, string color = "Black")
        {
            this.height = height;
            this.color = color;
        }

        public abstract IPrototype Clone();
    }

    class Floor : ArchitectObject
    {
        public Floor(int height) : base(height)
        {
        }

        public override IPrototype Clone()
        {
            throw new NotImplementedException();
        }
    }

    class Building : ArchitectObject
    {
        public List<Floor> floors;
        public int numberOfFloors;

        public Building(int nFloors = 5, int height = 2, string color = "Stupid")
        {
            floors = new List<Floor>();
            this.color = color;
            numberOfFloors = nFloors;
            for (int i = 0; i < nFloors; i++)
            {
                floors.Add(new Floor(height));
            }

            this.height = height * nFloors;
        }

        ~Building()
        {
            floors.Clear();
        }

        public override IPrototype Clone()
        {
            return new Building(numberOfFloors, height / numberOfFloors, color);
        }
    }

    class Trunk : ArchitectObject
    {
        public Trunk(int height, string color) : base(height, color)
        {
        }

        public override IPrototype Clone()
        {
            throw new NotImplementedException();
        }
    }

    class Foliage : ArchitectObject
    {
        public Foliage(int height, string color) : base(height, color)
        {
        }

        public override IPrototype Clone()
        {
            throw new NotImplementedException();
        }
    }

    class Tree : ArchitectObject
    {
        public Trunk trunk;
        public Foliage foliage;

        public Tree(int TrunkHeight = 4, string TrunkColor = "Brown",
            int FoliageHeight = 2, string FoliageColor = "Green")
        {
            trunk = new Trunk(TrunkHeight, TrunkColor);
            foliage = new Foliage(FoliageHeight, FoliageColor);
        }

        public override IPrototype Clone()
        {
            return new Tree(trunk.height, trunk.color, foliage.height, foliage.color);
        }
    }

    class Bench : ArchitectObject
    {
        public int width;

        public Bench(int height = 1, int width = 2, string color = "BenchColor") : base(
            height, color)
        {
            this.width = width;
        }

        public override IPrototype Clone()
        {
            return new Bench(height, width, color);
        }
    }
}