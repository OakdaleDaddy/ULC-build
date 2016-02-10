MEMORY
{
  /* Define each memory region */
  RAM_32k (rwx) : ORIGIN = 0x10000000, LENGTH = 0x8000 		/* 32k */
  RAM_8k  (rwx) : ORIGIN = 0x20000000, LENGTH = 0x2000 		/* 8k */
  RO_FLASH (rx) : ORIGIN = 0x00000000, LENGTH = 0x20000		/* Flash: 128k */
}
  /* Define a symbol for the top of each memory region */
  __TOP_RAM_32k  = 0x10000000 + 0x8000;
  __TOP_RAM_8k   = 0x20000000 + 0x2000;
  __TOP_RO_FLASH = 0x00000000 + 0x20000;


/* Entrypoint */
ENTRY(ResetISR)

SECTIONS
{

	__code_checksum = 0 - (_vStackTop + ResetISR + 1 + NMI_Handler + 1 + HardFault_Handler + 1 + MemManage_Handler + 1 + BusFault_Handler + 1 + UsageFault_Handler + 1);
	
	.ident 0x80 :
	{
		*(.ident)
	} > RO_FLASH
	
	.C2F_Info 0x100 :
	{
		*(.C2F_Info)
		
		SHORT ( 1);												/* Mode */
		SHORT ( 1);												/* Number of CRC Blocks */
		
		LONG ( 0x2000);										/* Begin of Firmware Block */
		LONG (( _etext + SIZEOF ( .data) - 0x2000 + 511) & ~(511));	/* Length of Firmware for CRC */
		SHORT ( 0);												/* Placeholder for CRC */
	} > RO_FLASH


	/* first setcion is the main text section for code  */	
	.text ALIGN(8192) : ALIGN(4)
	{
		FILL(0xff)
		KEEP(*(.isr_vector))
		
		/* Global Section Table */
		. = ALIGN(4) ;
		__section_table_start = .;
		__data_section_table = .;
		LONG(LOADADDR(.data));
		LONG(    ADDR(.data)) ;
		LONG(  SIZEOF(.data));
		LONG(LOADADDR(.data_RAM2));
		LONG(    ADDR(.data_RAM2)) ;
		LONG(  SIZEOF(.data_RAM2));
		__data_section_table_end = .;
		__bss_section_table = .;
		LONG(    ADDR(.bss));
		LONG(  SIZEOF(.bss));
		LONG(    ADDR(.bss_RAM2));
		LONG(  SIZEOF(.bss_RAM2));
		__bss_section_table_end = .;
		__section_table_end = . ;
		/* End of Global Section Table */
		

		*(.after_vectors*)
		
		*(.text*)
		*(.rodata .rodata.*)
		. = ALIGN(4);
		
	} > RO_FLASH
		
	_etext = .;


	/*
	 * for exception handling/unwind - some Newlib functions (in common
	 * with C++ and STDC++) use this. 
	 */
	.ARM.extab : ALIGN(4)
	{
		*(.ARM.extab* .gnu.linkonce.armextab.*)
	} > RAM_32k 
	__exidx_start = .;
	
	.ARM.exidx : ALIGN(4)
	{
		*(.ARM.exidx* .gnu.linkonce.armexidx.*)
	} > RAM_32k 
	__exidx_end = .;
	
	/* MAIN DATA SECTION */
	/*

	.uninit_RESERVED : ALIGN(4)
	{
		KEEP(*(.bss.$RESERVED*))
		. = ALIGN(4) ;
		_end_uninit_RESERVED = .;
	} > RAM_32k 
	*/
	
	.data : AT(_etext)  ALIGN(4) 
	{
		FILL(0xff)
		_data = .;
		*(vtable)
		*(.data*)
		. = ALIGN(4) ;
		_edata = .;
	} > RAM_32k

	.data_RAM2 : AT(_edata) ALIGN(4)
	{
	   FILL(0xff)
		*(.data.$RAM2*)
		*(.data.$RAM_8k*)
	   . = ALIGN(4) ;
	} > RAM_8k 
	
	/* MAIN BSS SECTION */
	.bss : ALIGN(4)
	{
		_bss = .;
		*(.bss*)
		*(COMMON)
		. = ALIGN(4) ;
		_ebss = .;
		PROVIDE(end = .);
	} > RAM_32k
			
	.bss_RAM2 : ALIGN(4)
	{
		*(.bss.$RAM2*)
		*(.bss.$RAM_8k*)
	   . = ALIGN(4) ;
	} > RAM_8k
		
	/*	
	.noinit_RAM2 (NOLOAD) : ALIGN(4)
	{
		*(.noinit.$RAM2*)
		*(.noinit.$RAM_8k*)
	   . = ALIGN(4) ;
	} > RAM_8k
	*/
	/* DEFAULT NOINIT SECTION */
	/*
	.noinit (NOLOAD): ALIGN(4)
	{
		_noinit = .;
		*(.noinit*) 
	 	. = ALIGN(4) ;
		_end_noinit = .;
	} > RAM_32k 
	*/

	PROVIDE(_pvHeapStart = .);
	PROVIDE(_vStackTop = __TOP_RAM_32k - 0);
}

