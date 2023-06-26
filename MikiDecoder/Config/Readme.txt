This folder contains 3 files:
SessionProgress.txt and SessionProgress.bak.txt
Config.txt

SessionProgress.txt contains two lines
- first - last checked password (iteration)
- second - limit, last iteration to do (for pool)

Confit.txt
Params you can set for MikiDecoder and 7z, Hashcat, JohnTheRipper.
You can configure password generator with this parameters:
#################### PASSSWORD CONFIGURATION ####################
# Characters in password
Characters=ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxzy0123456789
# Password Min length
PassMinLength=32
# Password Max length
PassMaxLength=32
########## MASK CONFIGRATION ##########
# Use Mask 0 - No, 1 - Yes
UseMask=0
# Mask config, from back ...?9?8?7?6?5?4?3?2?1, ie: ?1=abcd, ?2=12345, ?####=????????..... each mask in new line
# If no mask, all characters in Password configuration will be used
?1=0
?32=a
#################### GENERATOR CONFIGURATION ####################
# Increment base
IncrementBase=3
# Increment power 108
IncrementPower=108
# 0 - Password, 1 - Iteration, 2 - Number (Password -> Number)
Mode=1
# Counter display 1 = true, 0 = false (percent while generating pass list, allways dislpay 100%)
CounterDisplay=0
#################### CONFIGURATION 7z ####################
# 7z parameters
ZipParam=t
# 7z Processes count
ProcCount=50
# 7z Dictionary size
7zDictSize=1000
# 7z Speed calculate loop count
Speed7zLoop=100
#################### CONFIGURATION HASHCAT ####################
# Hascat parameters
HCParam=-m 11600 -w 4 --potfile-disable -a 0
# HC Dictionary size
HCDictSize=1000
#################### CONFIGURATION JOHN THE RIPPER ####################
# JohnTheRipper parameters
JTRParam=--format=7z
# JTR Dictionary size 6048
JTRDictSize=1000
#################### CONFIGURATON TEST MODE ####################
# Test mode with password checking 1 = true, 0 = false
PassCheck=1
# Test mode display speed after n loops
PassLoops=100000


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
