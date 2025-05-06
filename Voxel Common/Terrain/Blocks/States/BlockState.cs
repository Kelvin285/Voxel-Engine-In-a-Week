using GlmSharp;
using VoxelEngineCore.Terrain.Blocks.Registry;
using VoxelEngineCore.Terrain.Blocks.States.TextureHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VoxelEngineCore.Terrain.Blocks.States
{
    public class BlockState
    {
        public int GlobalID { get; private set; }

        public static List<Block> AllParents = new();
        public static List<bool> AllSolid = new();
        public static List<bool> AllVisible = new();
        public static List<bool> AllTransparent = new();
        public static List<vec3> AllMapColors = new();
        public static List<TextureHolderBase?> AllTextures = new();
        public static List<uint> AllBits = new();

        public Block Parent
        {
            get => AllParents[GlobalID]; set => AllParents[GlobalID] = value;
        }

        public bool Solid
        {
            get => AllSolid[GlobalID]; set => AllSolid[GlobalID] = value;
        }

        public bool Visible
        {
            get => AllVisible[GlobalID]; set => AllVisible[GlobalID] = value;
        }

        public bool Transparent
        {
            get => AllTransparent[GlobalID]; set => AllTransparent[GlobalID] = value;
        }

        public vec3 MapColor
        {
            get => AllMapColors[GlobalID]; set => AllMapColors[(int)GlobalID] = value;
        }

        public TextureHolderBase? TextureHolder
        {
            get => AllTextures[GlobalID]; set => AllTextures[GlobalID] = value;
        }

        public uint Bits
        {
            get => AllBits[GlobalID]; set => AllBits[GlobalID] = value;
        }

        public BlockState(Block parent, uint Bits)
        {
            AllParents.Add(parent);
            AllSolid.Add(true);
            AllVisible.Add(true);
            AllTransparent.Add(false);
            AllMapColors.Add(vec3.Zero);
            AllTextures.Add(null);
            AllBits.Add(Bits);

            GlobalID = AllBlocks.States.Count;
            AllBlocks.States.Add(this);
        }

        /// <summary>
        /// replace the current bits with the bits needed for this specific property value and then return the resulting blockstate
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public BlockState With(IBlockProperty property, Enum value)
        {
            var offset = Parent.PropertyToOffset[property];
            var b = Bits;
            uint test = ~((uint.MaxValue >> (32 - property.GetNumBits())) << offset);
            b &= test;
            b |= (uint)((int)(object)value << offset);
            return Parent.States[(int)b];
        }

        public int GetValue(IBlockProperty property)
        {
            var offset = Parent.PropertyToOffset[property];
            uint test = uint.MaxValue >> (32 - property.GetNumBits());
            var b = (Bits >> offset) & test;
            return (int)b;
        }
    }
}
