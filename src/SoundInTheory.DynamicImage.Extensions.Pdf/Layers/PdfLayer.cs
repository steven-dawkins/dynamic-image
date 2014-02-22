using System.IO;
using SoundInTheory.DynamicImage.Util;

namespace SoundInTheory.DynamicImage.Layers
{
	public class PdfLayer : Layer
	{
		public string SourceFileName
		{
			get { return this["SourceFileName"] as string ?? string.Empty; }
			set { this["SourceFileName"] = value; }
		}

		public int PageNumber
		{
			get { return (int)(this["PageNumber"] ?? 1); }
			set { this["PageNumber"] = value; }
		}

        public int Width
        {
            get { return (int)(this["Width"] ?? 96); }
            set { this["Width"] = value; }
        }

        public int Height
        {
            get { return (int)(this["Height"] ?? 96); }
            set { this["Height"] = value; }
        }

		public override bool HasFixedSize
		{
			get { return true; }
		}

		protected override void CreateImage(ImageGenerationContext context)
		{
			GhostscriptUtil.EnsureDll(context);

			string outputFileName = Path.GetTempFileName();

			try
			{
				string filename = FileSourceHelper.ResolveFileName(context, SourceFileName);
                GhostscriptWrapper.GeneratePageThumb(filename, outputFileName, PageNumber, Width, Height);
				Bitmap = new FastBitmap(File.ReadAllBytes(outputFileName));
			}
			finally
			{
				File.Delete(outputFileName);
			}
		}
	}
}
