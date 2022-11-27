This folder contains 3 files:
SessionProgress.txt and SessionProgress.bak.txt
Config.txt

SessionProgress.txt contains two lines
- first - last checked password (iteration)
- second - limit, last iteration to do (for pool)

Confit.txt
Params you can set for MikiDecoder and 7z, Hashcat, JohnTheRipper.
You can configure password generator with this parameters:
# Characters in password
Characters=ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxzy0123456789
# Password Min length
PassMinLength=32
# Password Max length
PassMaxLength=32
# Increment base
IncrementBase=3
# Increment power 108
IncrementPower=108
# 0 - Password, 1 - Iteration, 2 - Number (Password -> Number)
Mode=1

The working principle of the decoder is to convert a decimal number to a password, similar to converting a decimal number to hexadecimal, so:
0  is 0, and/or 00, and/or 000 a.s.o (depends on PassMinLength and PassMaxLength)
61 is z, and/or 0z, and/or 00z a.s.o
2272657884496751345355241563627544170162852933518655225855 is zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz (62^32)-1
(Decoder can work on very big string-integer-like numbers, you can test it in Calculator mode.)

When Mode=1 (default) program uses Iteration (look SessionProgress.txt) to generate next passwords, it is the best and only one way to distribute pools and avoid repetitions.

How it works:
Hashcat and JohnTheRipper mode - dictionary attack (folder Dictionary), when HC / JTR do their job, new Dictionary is generated...
7z mode - multitasking 7z job

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
	
Important!!! to avoid repetitions: TotalIterCount % Increment != 0

Program ends if last total iteration or last iteration form SessionProgress.txt is done.

If Hashcat or JohnTheRipper find password, program will check it with 7z. If password is False Positive, program will continue searching passwords (save FalsePositive, delete potfile, clean Dictionary, return to work).
