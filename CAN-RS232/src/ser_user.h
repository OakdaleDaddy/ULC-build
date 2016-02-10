
#ifndef _SER_USER_H_
#define _SER_USER_H_

u32_t serialInit(u32_t baud, u8_t dataBits, u8_t stopBits, u8_t parityCode);
u32_t serialWrite(u8_t * data, u8_t length);
u32_t serialRead(u8_t * data);

#endif

