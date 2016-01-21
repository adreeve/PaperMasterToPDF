Back in the 90s I made a decision to get rid of a lot of hard copy documentation and convert it to digital files that I could read on my PC while not consuming binders worth of space. I chose 
a nice product called PaperMaster to assist with that task and all went well. I scanned everything in (consuming much time) and eventually archived it all to a CD and things would stay that way
for over a decade. I didn't think I'd actually need any of the documents, but they were nice to have... and then I wanted to take a look at them for fun. PaperMaster 
would no longer run on my Windows 7 PC. It had changed ownership, versions, ... etc. I did find an export utility from another company, but that didn't work for me... my archive was probably 
too old for it. So I spent some time looking at the files. Eventually I figured out all of my documents were stored as single page TIFF images. I was able to assemble a few documents back 
together and store them in the now more common PDF file format. However, that task was tedious and I gave up after about 3 documents putting this project back on a shelf. Recently I picked it 
up again and decided to try to write some software to do the conversion for me. I spent time using HxD (a hex editor) analyzing the files making up each archive (or Cabinet as they called them) 
and was eventually able to decipher the format used... at least enough so to create this software for extracting all the documents out of an old PaperMaster Cabinet.

I'm publishing this to Git as I see others have looked for such a utility in the past. Maybe this will help someone. It's possible you'll have some PaperMaster files and this won't work because
you're using an upgraded (or older) version. I can only code based on my sample data. I had 3 Cabinets and this got all of my data out of them into PDF format. I added a few features, but there
are some notable absences such as no progress dialog when extracting. I also don't do any real error checking for invalid cabinets and such... again... for a one and done utility I didn't need
that. This is a one time thing and now that my files have been exported I don't need this anymore.

If you're curious about the PaperMaster file structure here's what I found:

Each cabinet has lots of files/folders in it, but the two root files we need are _DOCID._PS and _foldid._ps. The _PS files are all stored in a common (database?) format that begins with a
header that consists of 8 signature bytes, 54 unknown bytes, and a 16 bit field count.

Header
  8 byte signature identifying the file type (F4 44 53 00 67 45 23 01)
 54 Unknown bytes (the first 52 are 00 and then a 16 bit number apparently, but I don't know what they are nor use them)
  2 bytes for a 16-bit field found

Then, based on the # of fields as specified in the header there are that many fields as follows:

Fields
 28 bytes for the field name
  1 byte type
  1 byte type length
  2 byte (16 bit) record offset

Record
  2 byte (16 bit) length
  4 byte (32 bit) offset into this file for the start of this record
  Fields for this record as specified by the fields

The fields each have a type and from what I have determined it appears that 2 means a string with the appropriate length of chars (or 0 for a NULL terminated string), 4 means a 16-bit value, 
and 6 means a 32-bit (4 byte) value.

Once we establish that the file _DOCID._PS contains a list of all of the files in this cabinet and the file _foldid._ps contains a list of the 'drawers' or folders used by this cabinet
that each document is in.

From there we can estabish which subdirectory in the PaperMaster archive folder contains a given file however that subdirectory is just that... another level that will point to the
exact folder containing the index of and TIFF images we want. These files are named _PFC._PS which each use the standard format for _PS files described. The first _PFC._PS files contains
a list of all files in this 'drawer' or folder. We need to match up the one we are looking for. It then tells us which folder within this folder will contain our file. We then read that
_PFC._PS file and it tells us the files names of the pages of the TIFF images.

Once I have that I used PDFSharp (a free .NET PDF library) to create a PDF files from these TIFF images.

I may not do everything perfectly given that I had to reverse engineer the file format and I may have taken some shortcuts in that I assumed specific fields would be at specific levels (and no other
fields), but I didn't want to spend longer than necessary on this as I'm not sure anyone else will find it useful. However, if you were stuck like me and want to once again view some files
from an old PaperMaster Cabinet this may help solve your dilemma.

- Alan (areeve@reevesoft.com)
