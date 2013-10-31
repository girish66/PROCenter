﻿namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System;
    using Pillar.Agatha.Message;

    #endregion

    public class SaveDtoResponse<TDto> : SaveDtoResponse<TDto, Guid>, IHaveDataTransferObject where TDto : KeyedDataTransferObject
    {
        public object Dto
        {
            get { return DataTransferObject; }
        }

        public KeyedDataTransferObject GetDto ()
        {
            return DataTransferObject;
        }
    }
}