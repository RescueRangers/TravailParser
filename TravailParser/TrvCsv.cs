using System;
using FileHelpers;

namespace TravailParser
{
    [IgnoreFirst]
    [DelimitedRecord(" ")]
    public abstract class TrvCsv
    {
        public string Name;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime WorkStart;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime WorkEnd;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime Duration;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime PreparationDuration;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime StopDuration;

        [FieldOptional] [FieldConverter(ConverterKind.Date)]
        public DateTime WorkDuration;

        public string[] TheRest;

    }
}
