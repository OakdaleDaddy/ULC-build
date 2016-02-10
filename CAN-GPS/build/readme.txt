install: yagarto-bu-2.23.1_gcc-4.7.2-c-c++_nl-1.20.0_gdb-7.5.1_eabi_20121222.exe
 - default destination: C:\yagarto-20121222

install: yagarto-tools-20121018-setup.exe
 - default destination: C:\yagarto-tools-20121018

make sure path includes : C:\yagarto-20121222\bin;C:\yagarto-tools-20121018\bin

enter directory of build: .\DeliveryFirmware
run shell.bat to prompt
execute 'make clean' to clean
execute 'make all' to compile

binary file stored within \.out : example_can.bin

load firmware by installing jumper and powering board
 - status 1 flashes orange
 - status 2 solid orange

start PcanFlash.exe from hard disc (needs writable location when executed)
 - connect to CAN interface at 500kbs (bottom shows Connected to...)
 - select Application | Options
  - select PCAN-GPS for Hardware Profile
  - browse for binary file
  - click ok
 - select Module | Detect
 - select detected module
 - click green play button to program
 - exit