using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandParser : Singleton<CommandParser> {

    public List<string> commands = new List<string>
    {
        "move",
        "turn",
        "grab",
        "drop",
        "open",
        "return",
        "scan"
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ParseResults ParseFile(string rawCommands)
    {
        // Create results instance 
        ParseResults results = new ParseResults();

        int columnNum = 0;
        int rowNum = 0;
        string[] lines = rawCommands.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);       

        List<string[]> parsedCommands = new List<string[]>();

        foreach (string line in lines)
        {
            rowNum++;
            columnNum = 0;
            string[] args = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (args.Length > 0)
            {
                // Switch on the first word and begin checking subsequent word for valiity
                switch (args[0])
                {
                    case ("move"):
                        if (args.Length == 2)
                        {
                            int temp;
                            if (int.TryParse(args[1], out temp))
                                break;
                            else
                            {
                                results.error = "Unable to parse argument [" + args[1] + "] in line " + rowNum;
                                results.error_line = rowNum;
                                return results;
                            }
                        }
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'move' in line " + rowNum + ". Expected 1.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("turn"):
                        if (args.Length == 2)
                        {
                            if (args[1] == "left" || args[1] == "right" || args[1] == "around")
                                break;
                            else
                            {
                                results.error = "Unable to parse argument [" + args[1] + "] in line " + rowNum + ". Correct args are 'left', 'right', or 'around'";
                                results.error_line = rowNum;
                                return results;
                            }
                        }
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'turn' in line " + rowNum + ". Expected 1.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("grab"):
                        if (args.Length == 1)
                            break;
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'grab' in line " + rowNum + ". Expected 0.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("drop"):
                        if (args.Length == 1)
                            break;
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'drop' in line " + rowNum + ". Expected 0.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("open"):
                        if (args.Length == 2)
                        {
                            int temp;
                            if (args[1].Length == 4 && int.TryParse(args[1], out temp))
                                break;
                            else
                            {
                                results.error = "Unable to parse argument [" + args[1] + "] in line " + rowNum + ". must be a 4 digit integer.";
                                results.error_line = rowNum;
                                return results;
                            }
                        }
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'use' in line " + rowNum + ". Expected 1.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("return"):
                        if (args.Length == 1)
                            break;
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'drop' in line " + rowNum + ". Expected 0.";
                            results.error_line = rowNum;
                            return results;
                        }
                    case ("scan"):
                        if (args.Length == 1)
                            break;
                        else
                        {
                            results.error = "Incorrect number of arguments for command 'drop' in line " + rowNum + ". Expected 0.";
                            results.error_line = rowNum;
                            return results;
                        }
                    default:
                        results.error = "Unable to recognize command '" + args[0] + "' in line " + rowNum;
                        results.error_line = rowNum;
                        return results;
                }

                results.commands.Add(args);
            }
        }

        results.success = true;
        return results;
    }

}

public class ParseResults
{
    public bool success = false;
    public string error = "";
    public int error_line = -1;
    public List<string[]> commands;

    public ParseResults()
    {
        this.commands = new List<string[]>();
    }
}
