//========================================================================================
// RandomFonts by Daan Juttmann
// Created: 2017-11-07
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RandomFont
{
  class FontRandomizer
  {
    private string [] Fonts; // list of font names
    private static Random Generator = new Random ();


    // Constructor, sets default font list.
    public FontRandomizer ()
    {
      Fonts = new [] {"Courier New", "Times New Roman", "Arial"};
    }


    // Escape characters if they are an rtf control character.
    private string EscapeRtf (char c) {
      if (c == '\\' || c == '{' || c == '}')
        return "\\" + c;
      return c.ToString ();
    }

    
    // Load the font names from a text file.
    public bool LoadFonts (string fontFile)
    {
      try
      {
        Fonts = System.IO.File.ReadAllLines (fontFile);
      }
      catch (Exception ex)
      {
        Console.WriteLine ("Could not open font file");
        Console.WriteLine (ex.Message);
        return false;
      }
      return true;
    }


    // Generate a random integer representing a font from Fonts array.
    private int RandomFont ()
    {
      return Generator.Next (Fonts.Length);
    }


    // Write the header of the output rtf file.
    private void WriteHeader (StreamWriter stream) {
      stream.WriteLine ("{\\rtf1\\ansi\\deff0{\\fonttbl");
      for (int i = 0; i < Fonts.Length; i++)
      {
        stream.WriteLine ("{{\\f{0} {1}}}", i, Fonts [i]);
      }
      stream.WriteLine ("}");
    }


    // Write a line with randomized fonts for each character.
    private void WriteLine (StreamWriter stream, string s)
    {
      for (int i = 0; i < s.Length; i++)
      {
        stream.Write ("\\f");
        stream.Write (RandomFont ());
        stream.Write (' ');
        stream.Write (EscapeRtf (s [i]));
      }
      stream.Write ("\\line");
      stream.Write (Environment.NewLine);
    }


    // Write end of file
    private void WriteEndOfFile (StreamWriter stream)
    {
      stream.Write ("}");
    }


    // Write the text from streamIn to streamOut as rtf file with randomized fonts.
    public void WriteFile (StreamReader streamIn, StreamWriter streamOut)
    {
      WriteHeader (streamOut);
      while (!streamIn.EndOfStream)
      {
        WriteLine (streamOut, streamIn.ReadLine ());
      }
      WriteEndOfFile (streamOut);
    }
  }



  class RandomFont
  {
    // Return the extension from a file name or path + file name
    static string GetFileExtension (string s)
    {
      for (int i = s.Length - 1; i >= 0; i--)
      {
        switch (s [i])
        {
        case '.':
          return s.Substring (i, s.Length - i);
        case '\\':
        case '/':
          return "";
        default:
          break;
        }
      }
      return s;
    }

    
    // Parse the command line parameters.
    private static void ParseArguments (string [] args,
                                        ref string inputFile,
                                        ref string outputFile,
                                        ref string fontFile)
    {
      if (args.Length > 0)
        inputFile = args [0];
      for (int i = 1; i < args.Length; i++)
      {
        if (args [i] == "-o" && i + 1 < args.Length)
        { // set output file name
          i++;
          outputFile = args [i];
          if (GetFileExtension (outputFile).ToLower () != ".rtf")
            outputFile += ".rtf"; // make sure output has .rtf extension
        }
        if (args [i] == "-f" && i + 1 < args.Length)
        { // set font file name
          i++;
          fontFile = args [i];
        }
      }
    }


    // Main
    static void Main (string [] args)
    {
      string inputFile = "";         // input file name
      string outputFile = "out.rtf"; // output file name
      string fontFile = "";          // font list file name
      StreamReader streamIn;
      StreamWriter streamOut;

      ParseArguments (args, ref inputFile, ref outputFile, ref fontFile);
      FontRandomizer f = new FontRandomizer ();
      if (!f.LoadFonts (fontFile))
        return;
      try
      { // load input stream
        streamIn = new StreamReader (new FileStream (inputFile, FileMode.Open));
      }
      catch (Exception ex)
      {
        Console.WriteLine ("Could not open input file");
        Console.WriteLine (ex.Message);
        return;
      }
      try
      { // load output stream
        streamOut = new StreamWriter (new FileStream (outputFile, FileMode.Create));
      }
      catch (Exception ex)
      {
        Console.WriteLine ("Could not open output file");
        Console.WriteLine (ex.Message);
        streamIn.Dispose ();
        return;
      }
      f.WriteFile (streamIn, streamOut);
      streamIn.Dispose ();
      streamOut.Dispose ();
    }
  }
}
