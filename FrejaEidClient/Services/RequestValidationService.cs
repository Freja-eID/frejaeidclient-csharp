using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Authentication;
using Com.Verisec.FrejaEid.FrejaEidClient.Beans.Common;
using Com.Verisec.FrejaEid.FrejaEidClient.Enums;
using Com.Verisec.FrejaEid.FrejaEidClient.Exceptions;
using System;
using System.Collections.Generic;

namespace Com.Verisec.FrejaEid.FrejaEidClient.Services
{
    public static class RequestValidationService
    {

        public static void ValidateInitAuthRequest(InitiateAuthenticationRequest initiateAuthenticationRequest, TransactionContext transactionContext)
        {
            ValidateRequest(initiateAuthenticationRequest);
            ValidateUserInfoTypeAndUserInfo(initiateAuthenticationRequest.UserInfoType, initiateAuthenticationRequest.UserInfo, transactionContext);
            ValidateRequestedAttributes(initiateAuthenticationRequest.AttributesToReturn);
            ValidateRelyingPartyIdIsEmpty(initiateAuthenticationRequest.RelyingPartyId);
        }

        public static void ValidateResultRequest(AuthenticationResultRequest authenticationResultRequest)
        {
            ValidateRequest(authenticationResultRequest);
            ValidateReference(authenticationResultRequest.AuthRef);
            ValidateRelyingPartyIdIsEmpty(authenticationResultRequest.RelyingPartyId);
        }

        public static void ValidateResultsRequest(AuthenticationResultsRequest authenticationResultsRequest)
        {
            ValidateRequest(authenticationResultsRequest);
            ValidateRelyingPartyIdIsEmpty(authenticationResultsRequest.RelyingPartyId);
        }

        public static void ValidateCancelRequest(CancelAuthenticationRequest cancelRequest)
        {
            ValidateRequest(cancelRequest);
            ValidateReference(cancelRequest.AuthRef);
            ValidateRelyingPartyIdIsEmpty(cancelRequest.RelyingPartyId);
        }

        private static void ValidateRequest(RelyingPartyRequest relyingPartyRequest)
        {
            if (relyingPartyRequest == null)
            {
                throw new FrejaEidClientInternalException(message: "Request cannot be null value.");
            }
        }

        private static void ValidateRelyingPartyIdIsEmpty(string relyingPartyId)
        {
            if (relyingPartyId != null)
            {
                if (relyingPartyId.Equals(String.Empty))
                {
                    throw new FrejaEidClientInternalException(message: "RelyingPartyId cannot be empty.");
                }
            }
        }

        private static void ValidateUserInfoTypeAndUserInfo(UserInfoType userInfoType, string userInfo, TransactionContext transactionContext)
        {
            if (String.IsNullOrWhiteSpace(userInfo))
            {
                throw new FrejaEidClientInternalException(message: "UserInfo cannot be null or empty.");
            }
            if (transactionContext == TransactionContext.PERSONAL && userInfoType.Equals(UserInfoType.ORG_ID))
            {
                throw new FrejaEidClientInternalException(message: "UserInfoType ORG ID cannot be used in personal context.");
            }
        }

        private static void ValidateRequestedAttributes(SortedSet<AttributeToReturn> requestedAttributes)
        {
            if (requestedAttributes != null && requestedAttributes.Count == 0)
            {
                throw new FrejaEidClientInternalException(message: "RequestedAttributes cannot be empty.");
            }
        }

        private static void ValidateReference(string reference)
        {
            if (String.IsNullOrWhiteSpace(reference))
            {
                throw new FrejaEidClientInternalException(message: "Reference cannot be null or empty.");
            }
        }
    }
}