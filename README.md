# fixsubs
A small utility to fix extra spaces in OCR'd subtitle SRT files.

Build with vscode using .net core.  


An example of what this utility does:

```59
00:09:23,362 --> 00:09:27,594
We can't accept that ki nd of
money for doi ng nothi ng .
```

transforms to 

```59
00:09:23,362 --> 00:09:27,594
we can't accept that kind of 
money for doing nothing. 
```

If a "word" doesn't exist in the dictionary, it is left alone as is.
