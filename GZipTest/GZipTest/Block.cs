namespace GZipTest
{
	public class Block
	{
		
		public Block(int id, byte[] blockBytes)
		{
			Id = id;
			BlockBytes = blockBytes;
		}

		public int Id { get; }

		public byte[] BlockBytes { get; }

		public int BlockSize { get; set; } 
	}
}