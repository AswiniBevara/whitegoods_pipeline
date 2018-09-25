#include <time.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>
#include <stdbool.h>
#include <string.h>
#include <limits.h>
#include <curl/curl.h>
#include <applibs/log.h>

#include "utils.h"



/// <summary>
///		Convert a time_t to a string with a specific format
/// </summary>
void Utils_GetFormattedDateFromTime(char* buffer, size_t buffer_size, time_t integer) {
	if (buffer_size < 31) {
		Log_Debug("ERROR: The size of the buffer was not big enough to store formatted time\n");
		return;
	}
	memset(buffer, 0, buffer_size);
	strftime(buffer, buffer_size, "\"%Y-%m-%dT%H:%M:%S.0000000Z\"", gmtime(&integer));
}