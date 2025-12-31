// See https://aka.ms/new-console-template for more information

using Syncie.Data.Fragmentation;

var fragmentator = new DataFragmentator();
var fragFile = await fragmentator.FragmentateAsync(
    new FileInfo("lorem.txt"));
Console.WriteLine(fragFile);
Console.WriteLine(fragFile.Blocks![0]);
Console.WriteLine(Convert.ToHexString(fragFile.Blocks[0].Checksum));