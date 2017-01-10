using System;
using System.Collections.Generic;
namespace CommonCLR
{
	interface IFile
	{
		FInfo FileInfo { get; }
		FInfo Read();
		System.Collections.Generic.List<IFile> ReadFolder(bool includeBody);
		System.Collections.Generic.List<IFile> ReadFolder();
	}
}
