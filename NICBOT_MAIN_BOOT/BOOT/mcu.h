/**************************************************************************
PROJECT:     CANopen Bootloader for Atmel AT90CAN128 AVR Microcontroller
MODULE:      MCU.H
CONTAINS:    SFR Description file for ImageCraft ICCAVR and AT90CAN128
ENVIRONMENT: Compiled and tested with IAR EW for AVR on
             Atmel STK500/501 with AT90CAN128
             Compiler Version 5.11b
COPYRIGHT:   This version Embedded Systems Academy, Inc.
             Developed by Embedded Systems Academy, Inc.
VERSION:     $LastChangedDate: 2013-05-28 02:11:48 +0200 (Tue, 28 May 2013) $
             $LastChangedRevision: 2624 $
***************************************************************************/
#ifndef MCU_H
#define MCU_H

/*==========================*/
/* Predefined SFR Addresses */
/*==========================*/
#include <avr/io.h>

/*==========================*/
/* Bit Position Definitions */
/*==========================*/
/* PINA : Input Pins, Port A */
#define    PINA7    7
#define    PINA6    6
#define    PINA5    5
#define    PINA4    4
#define    PINA3    3
#define    PINA2    2
#define    PINA1    1
#define    PINA0    0

/* DDRA : Data Direction Register, Port A */
#define    DDA7     7
#define    DDA6     6
#define    DDA5     5
#define    DDA4     4
#define    DDA3     3
#define    DDA2     2
#define    DDA1     1
#define    DDA0     0


/* PORTA : Data Register, Port A */
#define    PA7      7
#define    PA6      6
#define    PA5      5
#define    PA4      4
#define    PA3      3
#define    PA2      2
#define    PA1      1
#define    PA0      0

/* PINB : Input Pins, Port B */
#define    PINB7    7
#define    PINB6    6
#define    PINB5    5
#define    PINB4    4
#define    PINB3    3
#define    PINB2    2
#define    PINB1    1
#define    PINB0    0

/* DDRB : Data Direction Register, Port B */
#define    DDB7     7
#define    DDB6     6
#define    DDB5     5
#define    DDB4     4
#define    DDB3     3
#define    DDB2     2
#define    DDB1     1
#define    DDB0     0

/* PORTB : Data Register, Port B */
#define    PB7      7
#define    PB6      6
#define    PB5      5
#define    PB4      4
#define    PB3      3
#define    PB2      2
#define    PB1      1
#define    PB0      0


/* PINC : Input Pins, Port C */
#define    PINC7    7
#define    PINC6    6
#define    PINC5    5
#define    PINC4    4
#define    PINC3    3
#define    PINC2    2
#define    PINC1    1
#define    PINC0    0

/* DDRC : Data Direction Register, Port C */
#define    DDC7     7
#define    DDC6     6
#define    DDC5     5
#define    DDC4     4
#define    DDC3     3
#define    DDC2     2
#define    DDC1     1
#define    DDC0     0

/* PORTC : Data Register, Port C */
#define    PC7      7
#define    PC6      6
#define    PC5      5
#define    PC4      4
#define    PC3      3
#define    PC2      2
#define    PC1      1
#define    PC0      0


/* PIND : Input Pins, Port D */
#define    PIND7    7
#define    PIND6    6
#define    PIND5    5
#define    PIND4    4
#define    PIND3    3
#define    PIND2    2
#define    PIND1    1
#define    PIND0    0

/* DDRD : Data Direction Register, Port D */
#define    DDD7     7
#define    DDD6     6
#define    DDD5     5
#define    DDD4     4
#define    DDD3     3
#define    DDD2     2
#define    DDD1     1
#define    DDD0     0

/* PORTD : Data Register, Port D */
#define    PD7      7
#define    PD6      6
#define    PD5      5
#define    PD4      4
#define    PD3      3
#define    PD2      2
#define    PD1      1
#define    PD0      0


/* PINE : Input Pins, Port E */
#define    PINE7    7
#define    PINE6    6
#define    PINE5    5
#define    PINE4    4
#define    PINE3    3
#define    PINE2    2
#define    PINE1    1
#define    PINE0    0

/* DDRE : Data Direction Register, Port E */
#define    DDE7     7
#define    DDE6     6
#define    DDE5     5
#define    DDE4     4
#define    DDE3     3
#define    DDE2     2
#define    DDE1     1
#define    DDE0     0

/* PORTE : Data Register, Port E */
#define    PE7      7
#define    PE6      6
#define    PE5      5
#define    PE4      4
#define    PE3      3
#define    PE2      2
#define    PE1      1
#define    PE0      0


/* PINF : Input Pins, Port F */
#define    PINF7    7
#define    PINF6    6
#define    PINF5    5
#define    PINF4    4
#define    PINF3    3
#define    PINF2    2
#define    PINF1    1
#define    PINF0    0

/* DDRF : Data Direction Register, Port F */
#define    DDF7     7
#define    DDF6     6
#define    DDF5     5
#define    DDF4     4
#define    DDF3     3
#define    DDF2     2
#define    DDF1     1
#define    DDF0     0

/* PORTF : Data Register, Port F */
#define    PF7      7
#define    PF6      6
#define    PF5      5
#define    PF4      4
#define    PF3      3
#define    PF2      2
#define    PF1      1
#define    PF0      0


/* Input Pins, Port G */
#define    PING4    4
#define    PING3    3
#define    PING2    2
#define    PING1    1
#define    PING0    0

/* DDRG : Data Direction Register, Port G */
#define    DDG4     4
#define    DDG3     3
#define    DDG2     2
#define    DDG1     1
#define    DDG0     0

/* PORTG : Data Register, Port G */
#define    PG4      4
#define    PG3      3
#define    PG2      2
#define    PG1      1
#define    PG0      0


/* TFR0 : Timer/Counter Interrupt Flag Register 0 */
#define    OCF0A    1
#define    TOV0     0

/* TFR1 : Timer/Counter Interrupt Flag Register 1 */
#define    ICF1     5
#define    OCF1C    3
#define    OCF1B    2
#define    OCF1A    1
#define    TOV1     0

/* TFR2 : Timer/Counter Interrupt Flag Register 2 */
#define    OCF2A    1
#define    TOV2     0

/* TFR3 : Timer/Counter Interrupt Flag Register 3 */
#define    ICF3     5
#define    OCF3C    3
#define    OCF3B    2
#define    OCF3A    1
#define    TOV3     0

/* EIFR : External Interrupt Flag Register */
#define    INTF7    7
#define    INTF6    6
#define    INTF5    5
#define    INTF4    4
#define    INTF3    3
#define    INTF2    2
#define    INTF1    1
#define    INTF0    0

/* EIMSK : External Interrupt Mask Register */
#define    INT7     7
#define    INT6     6
#define    INT5     5
#define    INT4     4
#define    INT3     3
#define    INT2     2
#define    INT1     1
#define    INT0     0

/* EECR : EEPROM Control Register */
#define    EERIE    3
#define    EEMWE    2
#define    EEWE     1
#define    EERE     0

/* GTCCR : General Timer Control Register */
#define    TSM      7
#define    PSR2     1
#define    PSR310   0


/* TCCR0A : Timer/Counter 0 Control Register */
#define    FOC0A     7
#define    WGM00    6
#define    COM0A1    5
#define    COM0A0    4
#define    WGM01    3
#define    CS02     2
#define    CS01     1
#define    CS00     0

/* SPCR : SPI Control Register */
#define    SPIE     7
#define    SPE      6
#define    DORD     5
#define    MSTR     4
#define    CPOL     3
#define    CPHA     2
#define    SPR1     1
#define    SPR0     0

/* SPSR : SPI Status Register */
#define    SPIF     7
#define    WCOL     6
#define    SPI2X    0

/* ACSR : Analog Comparator Control and Status Register */
#define    ACD      7
#define    ACBG     6
#define    ACO      5
#define    ACI      4
#define    ACIE     3
#define    ACIC     2
#define    ACIS1    1
#define    ACIS0    0

/* OCDR : On-Chip Debug Register */
#define    IDRD     7
#define    OCDR7    7
#define    OCDR6    6
#define    OCDR5    5
#define    OCDR4    4
#define    OCDR3    3
#define    OCDR2    2
#define    OCDR1    1
#define    OCDR0    0

/* SMCR : Sleep Mode Control Register */
#define    SM2      3
#define    SM1      2
#define    SM0      1
#define    SE       0

/* MCUSR : MCU general Status Register */
#define    JTRF     4
#define    WDRF     3
#define    BORF     2
#define    EXTRF    1
#define    PORF     0

/* MCUCR : MCU general Control Register */
#define    JTD      7
#define    PUD      4
#define    IVSEL    1
#define    IVCE     0

/* SPMCR : Store Program Memory Control and Status Register */
#define    SPMIE    7
#define    RWWSB    6
#define    RWWSRE   4
#define    BLBSET   3
#define    PGWRT    2
#define    PGERS    1
#define    SPMEN    0

/* RAMPZ : RAM Page Z Select Register */
#define    RAMPZ0   0

/* SPH : Stack Pointer High */
#define    SP15     7
#define    SP14     6
#define    SP13     5
#define    SP12     4
#define    SP11     3
#define    SP10     2
#define    SP9      1
#define    SP8      0

/* SPL : Stack Pointer Low */
#define    SP7      7
#define    SP6      6
#define    SP5      5
#define    SP4      4
#define    SP3      3
#define    SP2      2
#define    SP1      1
#define    SP0      0

/* WTDCR : Watchdog Timer Control Register */
#define    WDCE     4
#define    WDE      3
#define    WDP2     2
#define    WDP1     1
#define    WDP0     0

/* CLKPR : Source Clock Prescaler Register */
#define    CKLPCE   7
#define    CKLPS3   3
#define    CKLPS2   2
#define    CKLPS1   1
#define    CKLPS0   0

/* TIMSK0 : Timer Interrupt Mask Register0 */
#define    OCIE0A   1
#define    TOIE0    0

/* TIMSK1 : Timer Interrupt Mask Register1 */
#define    ICIE1    5
#define    OCIE1C   3
#define    OCIE1B   2
#define    OCIE1A   1
#define    TOIE1    0

/* TIMSK2 : Timer Interrupt Mask Register2 */
#define    OCIE2A   1
#define    TOIE2    0

/* TIMSK3 : Timer Interrupt Mask Register3 */
#define    ICIE3    5
#define    OCIE3C   3
#define    OCIE3B   2
#define    OCIE3A   1
#define    TOIE3    0

/* XMCRA : External Memory Control A Register */
#define    SRE      7
#define    SRL2     6
#define    SRL1     5
#define    SRL0     4
#define    SRW11    3
#define    SRW10    2
#define    SRW01    1
#define    SRW00    0

/* XMCRB : External Memory Control B Register */
#define    XMBK     7
#define    XMM2     2
#define    XMM1     1
#define    XMM0     0

/* ADCSRA : ADC Control and Status Register A*/
#define    ADEN     7
#define    ADSC     6
#define    ADRF     5
#define    ADIF     4
#define    ADIE     3
#define    ADPS2    2
#define    ADPS1    1
#define    ADPS0    0

/* ADCSRB : ADC Control and Status Register B*/
#define    ADCHSM   7
#define    ACME     6
#define    ADST2    2
#define    ADST1    1
#define    ADST0    0

/* ADMUX : ADC Multiplexer Selection Register */
#define    REFS1    7
#define    REFS0    6
#define    ADLAR    5
#define    MUX4     4
#define    MUX3     3
#define    MUX2     2
#define    MUX1     1
#define    MUX0     0

/* TCCR1A : Timer/Counter 1 Control Register A */
#define    COM1A1   7
#define    COM1A0   6
#define    COM1B1   5
#define    COM1B0   4
#define    COM1C1   3
#define    COM1C0   2
#define    WGM11    1
#define    WGM10    0

/* TCCR1B : Timer/Counter 1 Control Register B */
#define    ICNC1    7
#define    ICES1    6
#define    WGM13    4
#define    WGM12    3
#define    CS12     2
#define    CS11     1
#define    CS10     0

/* TCCR1C : Timer/Counter 1 Control Register C */
#define    FOC1A    7
#define    FOC1B    6
#define    FOC1C    5

/* TCCR3A : Timer/Counter 3 Control Register A */
#define    COM3A1   7
#define    COM3A0   6
#define    COM3B1   5
#define    COM3B0   4
#define    COM3C1   3
#define    COM3C0   2
#define    WGM31    1
#define    WGM30    0

/* TCCR3B : Timer/Counter 3 Control Register B */
#define    ICNC3    7
#define    ICES3    6
#define    WGM33    4
#define    WGM32    3
#define    CS32     2
#define    CS31     1
#define    CS30     0

/* TCCR3C : Timer/Counter 3 Control Register C */
#define    FOC3A    7
#define    FOC3B    6
#define    FOC3C    5

/* TCCR2A : Timer/Counter 2 Control Register A*/
#define    FOC2     7
#define    WGM20    6
#define    COM21    5
#define    COM20    4
#define    WGM21    3
#define    CS22     2
#define    CS21     1
#define    CS20     0

/* ASSR : Asynchronous mode Status Register */
#define    EXCLK    4
#define    AS2      3
#define    TCN2UB   2
#define    OCR2UB   1
#define    TCR2UB   0

/* TWSR : TWI Status Register */
#define    TWS7     7
#define    TWS6     6
#define    TWS5     5
#define    TWS4     4
#define    TWS3     3
#define    TWPS1    1
#define    TWPS0    0

/* TWAR : TWI (slave) Address Register */
#define    TWA6     7
#define    TWA5     6
#define    TWA4     5
#define    TWA3     4
#define    TWA2     3
#define    TWA1     2
#define    TWA0     1
#define    TWGCE    0

/* TWCR : TWI Control Register */
#define    TWINT    7
#define    TWEA     6
#define    TWSTA    5
#define    TWSTO    4
#define    TWWC     3
#define    TWEN     2
#define    TWIE     0

/* UCSR0A : USART0 Control and Status Register A */
#define    RXC0     7
#define    TXC0     6
#define    UDRE0    5
#define    FE0      4
#define    DOR0     3
#define    UPE0     2
#define    U2X0     1
#define    MPCM0    0

/* UCSR0B : USART0 Control and Status Register B */
#define    RXCIE0   7
#define    TXCIE0   6
#define    UDRIE0   5
#define    RXEN0    4
#define    TXEN0    3
#define    UCSZ02   2
#define    RXB80    1
#define    TXB80    0

/* UCSR0C : USART0 Control and Status Register C */
#define    UMSEL0   6
#define    UPM01    5
#define    UPM00    4
#define    USBS0    3
#define    UCSZ01   2
#define    UCSZ00   1
#define    UCPOL0   0

/* UCSR1A : USART1 Control and Status Register A */
#define    RXC1     7
#define    TXC1     6
#define    UDRE1    5
#define    FE1      4
#define    DOR1     3
#define    UPE1     2
#define    U2X1     1
#define    MPCM1    0

/* UCSR1B : USART1 Control and Status Register B */
#define    RXCIE1   7
#define    TXCIE1   6
#define    UDRIE1   5
#define    RXEN1    4
#define    TXEN1    3
#define    UCSZ12   2
#define    RXB81    1
#define    TXB81    0

/* UCSR1C : USART1 Control and Status Register C */
#define    UMSEL1   6
#define    UPM11    5
#define    UPM10    4
#define    USBS1    3
#define    UCSZ11   2
#define    UCSZ10   1
#define    UCPOL1   0

/* CANGCON : CAN General Control Register */
#define    ABRQ     7
#define    OVRQ     6
#define    TTC      5
#define    SYNCTTC  4
#define    LISTEN   3
#define    ENA      1
#define    SWRES    0

/* CANGSTA : CAN General Status Register */
#define    OVFG     6
#define    TXBSY    4
#define    RXBSY    3
#define    ENFG     2
#define    BOFF     1
#define    ERRP     0

/* CANGIT : CAN General Interrupt Register */
#define    CANIT    7
#define    BOFFIT   6
#define    OVRTIM   5
#define    OVRBUF   4
#define    SERG     3
#define    CERG     2
#define    FERG     1
#define    AERG     0

#define    ERR_GEN_msk   0x0F
#define    INT_GEN_msk   0x5F

/* CANGIE : CAN General Interrupt Enable Register */
#define    ENIT     7
#define    ENBOFF   6
#define    ENRX     5
#define    ENTX     4
#define    ENERMOB  3
#define    ENBUF    2
#define    ENERG    1
#define    ENOVRT   0

/* CANBT1: CAN Bit Timing 1 Register */
#define    BRP      1
#define    BRP_msk  0x7E

/* CANBT2: CAN Bit Timing 2 Register */
#define    SJW      5
#define    SJW_msk  0x60
#define    PRS      1
#define    PRS_msk  0x0E

/* CANBT3: CAN Bit Timing 3 Register */
#define    PHS2     4
#define    PHS2_msk 0x70
#define    PHS1     1
#define    PHS1_msk 0x0E
#define    SMP      0

/* CANHPMOB : CAN Highest Priority MOB Register */
#define    HPMOB    4
#define    HPMOB_msk 0xF0

/* CANPAGE : CAN MOB Page Register */
#define    AINC     3
#define    MOBNB    4
#define    MOBNB_msk 0xF0

/* CANSTMOB : CAN MOB Status Register */
#define    DLCW     7
#define    TXOK     6
#define    RXOK     5
#define    BERR     4
#define    SERR     3
#define    CERR     2
#define    FERR     1
#define    AERR     0

#define    ERR_MOB_msk   0x1F
#define    INT_MOB_msk   0x7F

/* CANCDMOB : CAN MOB Control Register */
#define    CONMOB   6
#define    CONMOB_msk 0xC0

#define    RPLY     5
#define    IDE      4
#define    DLC      0
#define    DLC_msk 0x0F

/* CANIDT4 : CAN Identifier Tag 4 Register */
#define    RTRTAG   2
#define    RB1TAG   1
#define    RB0TAG   0

/* CANIDM4 : CAN Identifier Mask 4 Register */
#define    RTRMSK   2
#define    IDEMSK   0

/* Pointer definition */
#define    XL     r26
#define    XH     r27
#define    YL     r28
#define    YH     r29
#define    ZL     r30
#define    ZH     r31

#endif  /* _MCU_H*/

/*******************************************************************************
END OF FILE
*******************************************************************************/














