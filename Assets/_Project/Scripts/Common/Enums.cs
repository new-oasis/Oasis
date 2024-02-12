namespace Oasis.Common
{
    public enum Side : byte
    {
        East,
        Up,
        North,
        West,
        Down,
        South
    } // %3 => xyz;  // Voxels have sides

    public enum BlockType
    {
        Air,
        Cube,
        Model
    }

    public enum TextureType
    {
        None,
        Opaque,
        Transparent,
        Cutout
    }
    public enum WorldType
    {
        Flat,
        Hills
    }
}