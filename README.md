# MikiDecoder

Miki Calculator and Pseudo-Random Password Generator for decoding 7z (Qlocker)...

Full description in MikiDecoder.docx

Calculator for big integer like string-numbers, string-number can have 2 147 483 647 digits. It can be more, but for now enough... ;-)
For Windows and Linux...

Please, read Readme.txt in each folder.
Now full configurable, constant or 1-n length password, mask for each possition, generator configuration, dictionary size for different kind of job, a.s.o,
Contains simple Calculator, Random (with reverse checking) Calculator for checking if program makes good calculations, Generator test (with reverse checking) to check
if password are generated without errors.

Speed ;-)... now, Release, on i7-9750H CPU @ 2.60GHz, 32 char password, 0-9+A-Z+a-z, constant length without mask:
about 170k/s

The working principle of the decoder is to convert a decimal number to a password, similar to converting a decimal number to hexadecimal, so:
0  is 0, and/or 00, and/or 000 a.s.o (depends on PassMinLength and PassMaxLength)
61 is z, and/or 0z, and/or 00z a.s.o
2272657884496751345355241563627544170162852933518655225855 is zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz (62^32)-1
(Decoder can work on very big string-integer-like numbers, you can test it in Calculator mode.)

How it works:
For HC, JTR and 7z program creates dictionaries, and saves them to the file.
Always there are two dictionaries, Dictionary.txt and DictionaryNext.txt, the second is created while the first is in the process of checking, 
so checking is a continuous process without waiting for a new dictionary.

7z mode - multitasking 7z job, you can set in Config.txt or during decoding with +,-,*,/ keys how many 7z processes MikiDecoder will launch.
If 7z mode is running, each 7z task in Silent mode works in Fire-and-forget manner, but in Display mode I made something like FIFO, it is a little slower
but you can see each password in the order of generation.

This pseudo-random generator uses Lehmer algorithm: NextDig = (LastDig + Increment) % TotalIterCount
but also:
IterToDo = LastCheckedIteration+1 (look SessionProgress.txt)
NextDig = (IterToDo * Increment) % TotalIterCount

Increment=IncrementBase^IncrementPower
TotalIterCount = Number of possible passwords (if 00 to 99, TotalIterCount = 100)

if Characters=0123456789 and PassMinLength=PassMaxLength=1, TotalIterCount = 10
if Characters=0123456789 and PassMinLength=PassMaxLength=32, TotalIterCount = 10^32
if Characters=0123456789 and PassMinLength=1 and PassMaxLength=5, TotalIterCount = 10^5, because the same iteration is used for all pass length:
if Increment = 3^1 = 3 then:
	00000, 0000, 000, 00, 0 - iteration 0
	00003, 0003, 003, 03, 3 - iteration 1
	00006, 0006, 006, 06, 6 - iteration 2
	99999                   - last total iteration (10^5)-1
	
Important!!! to avoid repetitions: TotalIterCount % Increment != 0, Increment must be prime number.

Program ends if last total iteration or last iteration form SessionProgress.txt is done.

If Hashcat or JohnTheRipper find password, program will check it with 7z. If password is False Positive, program will continue searching passwords (save FalsePositive, delete potfile, clean Dictionary, return to work).
