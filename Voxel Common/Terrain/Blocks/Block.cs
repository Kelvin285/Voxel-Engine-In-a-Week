using GlmSharp;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States;
using VoxelEngineCore.Terrain.Blocks.States.PropertyEnums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks
{
    public abstract class Block
    {

        public string Name { get; private set; }
        public int GlobalId { get; private set; }


        public Dictionary<int, IBlockProperty> OffsetToProperty = new();
        public Dictionary<IBlockProperty, int> PropertyToOffset = new();

        public List<BlockState> States = new();
        public BlockState DefaultState => States[0];

        private int TotalBits = 0;
        public Block(string name)
        {
            Name = name;
            GlobalId = AllBlocks.Blocks.Count;
            AllBlocks.Blocks.Add(this);

            BuildPropertyTable();
            BuildBlockStates();
        }

        public virtual void BuildPropertyTable()
        {
            AddProperty(new BlockProperty<DefaultProperty>("Default"));
        }

        public abstract void BuildBlockState(BlockState state);

        public void BuildBlockStates()
        {
            for (uint i = 0; i < glm.Pow(2, (uint)TotalBits); i++)
            {
                BlockState state = new(this, i);
                BuildBlockState(state);
                States.Add(state);
            }
        }

        public Enum GetPropertyValue(IBlockProperty property, uint bits)
        {
            var offset = PropertyToOffset[property];

            bits = bits >> offset;
            bits &= uint.MaxValue << property.GetNumBits();
            return property.GetValue((int)bits);
        }

        public void AddProperty(IBlockProperty property)
        {
            OffsetToProperty.Add(TotalBits, property);
            PropertyToOffset.Add(property, TotalBits);
            TotalBits += property.GetNumBits();
        }
    }
}
