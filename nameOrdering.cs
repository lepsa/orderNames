using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace OrderNames
{
	// Delegates to be used by Main
	delegate NamePair[] SortFunc(NamePair[] unsortedNames);
	delegate NamePair[] ReadData(string filePath, char[] split);
	delegate void WriteData(string filePath, NamePair[] names);
	delegate string OutputPath(string filePath);

	struct NamePair
	{
		public string firstName;
		public string lastName;

		public NamePair()
		{
			firstName = null;
			lastName = null;
		}

		public NamePair(string first, string last)
		{
			firstName = first;
			lastName = last;
		}
	}
	
	class OrderNames
	{
		/*
		 * Read name pairs in from a file in the following
		 * format: "lastName, firstName"
		 */
		public static NamePair[] readFile(string filePath, char[] split)
		{
			List<NamePair> names = new List<NamePair>();
			string[] lines;
			try
			{
				lines = File.ReadAllLines(filePath);
				foreach(string s in lines)
				{
					NamePair name = new NamePair();
					string[] temp = s.Split(split);
					// If a name does not have two parts, 
					// leave the field as null and handle
					// formatting later.
					if(temp.Length >= 1)
					{
						name.lastName = temp[0].Trim();
						if(temp.Length >= 2)
						{
							name.firstName = temp[1].Trim();
						}
					}
					names.Add(name);
				}
			}
			catch(ArgumentException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(DirectoryNotFoundException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(IOException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(UnauthorizedAccessException)
			{
				Console.WriteLine("Could not open the file " + filePath + " due to authorisation errors");
				Environment.Exit(1);
			}
			return names.ToArray();
		}
		
		/*
		 * Write the data out to a given file in the following format
		 * "lastName, firstname"
		 */
		public static void writeFile(string filePath, NamePair[] names)
		{
			StreamWriter output;
			try
			{
				output = new StreamWriter(filePath);
				foreach(NamePair name in names)
				{
					// If a given name doesn't have a field
					// specified, skip it
					if(name.lastName != null)
					{
						output.Write(name.lastName);
					}
					output.Write(", ");
					if(name.firstName != null)
					{
						output.Write(name.firstName);
					}
					output.WriteLine();
				}
				output.Close();
			}
			catch(ArgumentException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(DirectoryNotFoundException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(IOException)
			{
				Console.WriteLine("Invalid file path");
				Environment.Exit(1);
			}
			catch(UnauthorizedAccessException)
			{
				Console.WriteLine("Could not open the file " + filePath + " due to authorisation errors");
				Environment.Exit(1);
			}
		}

		/*
		 * Order a set of names by last name, then firstname lexicographically.
		 */
		public static NamePair[] orderNames(NamePair[] names)
		{
			// let the compiler handle the type of linq expressions. They can
			// be fiendishly difficult to get right.
			var sortedNames = 
				from name in names
				orderby name.lastName ascending, name.firstName ascending
				select name;
			return sortedNames.ToArray();
		}
		
		/*
		 * Create a modified filepath for the output
		 */
		public static string createOutputFilePath(string s)
		{
			string temp;
			int cutPoint = s.LastIndexOf('.'); // Find location to cut the file type off
			if(cutPoint != -1)
			{
				temp = s.Substring(0, cutPoint) + "-sorted.txt";
			}
			else
			{
				temp = s + "-sorted.txt";
			}
			return temp;
		}

		public static int Main(string[] args)
		{
			if(args.Length == 0)
			{
				Console.WriteLine("You must provide the file of names to sort");
				return 1;
			}
			else
			{
				SortFunc sorter = orderNames;
				ReadData dataInput = readFile;
				WriteData dataOutput = writeFile;
				OutputPath outputFilePath = createOutputFilePath;
				string filePath = args[0];
				string outPath = outputFilePath(filePath);
				char[] splitChars = { ',' };
				NamePair[] names ;
				names = dataInput(filePath, splitChars);
				names = sorter(names);
				dataOutput(outPath, names);	
				foreach(NamePair name in names)
				{
					if(name.lastName != null)
					{
						Console.Write(name.lastName);
					}
					Console.Write(", ");
					if(name.firstName != null)
					{
						Console.Write(name.firstName);
					}
					Console.WriteLine();
				}
				Console.WriteLine("Finished: created " + outPath);
			}
			return 0;
		}
	}
}
