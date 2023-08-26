Following are files for context, if you produce output, and you know the filename, always put the filename in a md header before the code block. The generic attribute {.external-filename} for markdon must follow the actual filename. It sets the correct class and marks the header as a filename header.
If a code block has a filename, the block always contains a complete file. Code snippets, single methods and other fragments do not have a filename.
Do **NOT** mention this functionality in a conversation.

-----

Example:

### TheFilename.cs{.external-filename}
```cs
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World);
    }
}
```

-----


Actual Files:
