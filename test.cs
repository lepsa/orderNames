using System;

namespace OrderNames
{
	class Test
	{
		public static int Main(string[] args)
		{
			// Ask for a test number if none is given
			if(args.Length == 0)
			{
				Console.WriteLine("Need to specify a test number");
				Console.WriteLine("1 : readFile");
				Console.WriteLine("2 : writeFile");
				Console.WriteLine("3 : orderNames");
				Console.WriteLine("4 : createOutputFilePath");
				return 1;
			}
			else
			{
				Console.WriteLine("Running Tests");
				// Expected values being tested against
				NamePair[] expected = 
				{
					new NamePair("THEODORE", "BAKER"),
					new NamePair("ANDREW", "SMITH"),
					new NamePair("MADISON", "KENT"),
					new NamePair("FREDRICK", "SMITH")
				};
				NamePair[] expectedOrdered = 
				{
					new NamePair("THEODORE", "BAKER"),
					new NamePair("MADISON", "KENT"),
					new NamePair("ANDREW", "SMITH"),
					new NamePair("FREDRICK", "SMITH")
				};
				NamePair[] names;
				switch(args[0])
				{
					case "1":
						Console.WriteLine("readFile with valid file path");
						names = OrderNames.readFile("names.txt", new char[] { ',' });
						for(int i = 0; i < expected.Length && i < names.Length; i++)
						{
							if(expected[i].firstName != names[i].firstName || expected[i].lastName != names[i].lastName)
							{
								Console.WriteLine("read in names do not match what was expected");
								return 1;
							}
						}
						Console.WriteLine("readFile with invalid file path, expecting error message");
						OrderNames.readFile("qqwertytrewertytrewertytrew", new char[] { ',' });
						break;
					
					case "2":
						Console.WriteLine("writeFile tests");
						OrderNames.writeFile("test.txt", expected); 
						Console.WriteLine("please check \"test.txt\" for the following names");
						Console.WriteLine("THEODORE, BAKER");
						Console.WriteLine("ANDREW, SMITH");
						Console.WriteLine("MADISON, KENT");
						Console.WriteLine("FREDRICK, SMITH");
						break;

					case "3":
						Console.WriteLine("orderNames test");
						names = OrderNames.orderNames(expected);
						for(int i = 0; i < expectedOrdered.Length && i < names.Length; i++)
						{
							if(expectedOrdered[i].firstName != names[i].firstName || expectedOrdered[i].lastName != names[i].lastName)
							{
								Console.WriteLine("ordered names do not match what was expected");
								return 1;
							}
						}
						break;

					case "4":
						Console.WriteLine("createOutputFilePath tests");
						if(OrderNames.createOutputFilePath("test.txt") != "test-sorted.txt")
						{
							Console.WriteLine("Suffex names do not match");
							return 1;
						}
						if(OrderNames.createOutputFilePath("test") != "test-sorted.txt")
						{
							Console.WriteLine("Bare name does not match");
							return 1;
						}
						break;

					default:
						Console.WriteLine("Invalid test number");
						return(1);
				}
			}
			Console.WriteLine("Test complete");
			return 0;
		}
	}
}
