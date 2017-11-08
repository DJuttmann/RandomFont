=========================================================================================
RandomFonts by Daan Juttmann
Created: 2017-11-07
License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
=========================================================================================

-- DESCRIPTION --
A program to convert an ANSI text file into an RTF file where each letter is displayed
with a random font. You can specify an input file, font list, and output file.

-- USAGE --
Command line parameters:
  RandomFonts.exe <input file> [-o <output file>] [-f <font file>]
If output file does not have .rtf extension, the .rtf extension will be added.
The font file is a text file with one font name on each line.
The output file should work with any reader supporting RTF files, provided that the
fonts used are installed on the system.

=========================================================================================