using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.ViewModels.Client;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetBanking.Core.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISavingsAccountService _savingsAccountService;
        private readonly ICreditCardService _creditCardService;
        private readonly ILoanService _loanService;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ITransactionService _transactionService;
        private AuthenticationResponse user;

        public ClientService(
            IAccountService accountService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISavingsAccountService savingsAccountService,
            ICreditCardService creditCardService,
            ILoanService loanService,
            IBeneficiaryService beneficiaryService,
            ITransactionService transactionService)
        {
            _accountService = accountService;
            _beneficiaryService = beneficiaryService;
            _creditCardService = creditCardService;
            _transactionService = transactionService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loanService = loanService;
            _savingsAccountService = savingsAccountService;
            user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync()
        {
            GetAllProductsByClientViewModel vm = new()
            {
                SavingsAccounts = await _savingsAccountService.GetByOwnerIdAsync(user.Id),
                CreditCards = await _creditCardService.GetByOwnerIdAsync(user.Id),
                Loans = await _loanService.GetByOwnerIdAsync(user.Id)
            };

            return vm;
        }


        public async Task<GetAllProductsByClientViewModel> GetAllProductsByClientAsync(string IdUser)
        {
            GetAllProductsByClientViewModel vm = new()
            {
                SavingsAccounts = await _savingsAccountService.GetByOwnerIdAsync(IdUser),
                CreditCards = await _creditCardService.GetByOwnerIdAsync(IdUser),
                Loans = await _loanService.GetByOwnerIdAsync(IdUser)
            };

            return vm;
        }


        public async Task<List<BeneficiaryViewModel>> GetAllBeneficiariesByClientAsync()
        {
            return await _beneficiaryService.GetByOwnerIdAsync(user.Id);
        }

        public async Task<TransactionStatusViewModel> RealizeTransaction(SaveTransactionViewModel svm)
        {
            var emissorProduct = await GetProductByIdAsync(svm.EmissorProductId);
            var receiverProduct = await GetProductByIdAsync(svm.ReceiverProductId);

            if (emissorProduct.Id != null && receiverProduct.Id != null)
            {

                #region Determina a donde enviar la actualizacion del emissorProduct

                int Identificator = Convert.ToInt32(svm.EmissorProductId.Substring(0, 3));


                if (100 <= Identificator && Identificator <= 299) //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(emissorProduct.Id);
                    if (svm.Type == TransactionType.CashAdvance)
                        creditCard.Amount += svm.Cantity + svm.Cantity * (decimal)(6.25 / 100);
                    else
                        creditCard.Amount += svm.Cantity; //Se le suma a su deuda


                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);

                }

                else if (300 <= Identificator && Identificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(emissorProduct.Id);
                    savingAccount.Amount -= svm.Cantity; //Se le resta a su dinero
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(savingAccount), savingAccount.Id);
                }

                else if (600 <= Identificator && Identificator <= 999) //Prestamos comienzan con 3 digitos entre 600 y 999
                {
                    var loan = await _loanService.GetByIdAsync(emissorProduct.Id);
                    loan.Amount += svm.Cantity;
                    await _loanService.UpdateAsync(_mapper.Map<SaveLoanViewModel>(loan), loan.Id);
                }
                #endregion


                #region Determina a donde enviar la actualizacion del receiverProduct

                Identificator = Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3));

                if (100 <= Identificator && Identificator <= 299)  //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(receiverProduct.Id);
                    if (creditCard.Amount - svm.Cantity < 0)
                    {
                        var sobra = svm.Cantity - creditCard.Amount;
                        svm.Cantity = svm.Cantity - sobra;

                        SaveTransactionViewModel retorno = new()
                        {
                            Cantity = sobra,
                            EmissorProductId = receiverProduct.Id,
                            ReceiverProductId = emissorProduct.Id
                        };
                        await RealizeTransaction(retorno);
                    }
                    creditCard.Amount -= svm.Cantity; //Se le resta a su deuda
                    await _creditCardService.UpdateAsync(_mapper.Map<SaveCreditCardViewModel>(creditCard), creditCard.Id);

                }

                else if (300 <= Identificator && Identificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(receiverProduct.Id);
                    savingAccount.Amount += svm.Cantity; //Se le suma a su dinero
                    await _savingsAccountService.UpdateAsync(_mapper.Map<SaveSavingsAccountViewModel>(savingAccount), savingAccount.Id);

                }
                else if (600 <= Identificator && Identificator <= 999) //Prestamos comienzan con 3 digitos entre 600 y 999
                {
                    var loan = await _loanService.GetByIdAsync(svm.ReceiverProductId);

                    if (loan.Amount - svm.Cantity < 0)
                    {
                        var sobra = svm.Cantity - loan.Amount;
                        svm.Cantity = svm.Cantity - sobra;

                        SaveTransactionViewModel retorno = new()
                        {
                            Cantity = sobra,
                            EmissorProductId = receiverProduct.Id,
                            ReceiverProductId = emissorProduct.Id,
                            Type = TransactionType.Leftover
                        };
                        await RealizeTransaction(retorno);
                    }
                    loan.Amount -= svm.Cantity; //Reduce la deuda
                    await _loanService.UpdateAsync(_mapper.Map<SaveLoanViewModel>(loan), loan.Id);
                }
                await _transactionService.AddAsync(svm);

                #endregion

            }

            return new TransactionStatusViewModel()
            {
                HasError = false
            };

        }

        public async Task<TransactionStatusViewModel> TransactionValidation(SaveTransactionViewModel svm)
        {
            var emissorProduct = await GetProductByIdAsync(svm.EmissorProductId);
            var receiverProduct = await GetProductByIdAsync(svm.ReceiverProductId);
            if (receiverProduct == null)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Cuenta destino inexistente."
                };
            }
            else if (svm.Cantity < 1)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Monto invalido."
                };
            }
            else if (emissorProduct.Id != null && receiverProduct.Id != null)
            {
                #region Posibles errores al realizar transaccion

                int emissorIdentificator = Convert.ToInt32(svm.EmissorProductId.Substring(0, 3));
                int receiverIdentificator = Convert.ToInt32(svm.ReceiverProductId.Substring(0, 3));


                if (100 <= emissorIdentificator && emissorIdentificator <= 299) //Tarjetas de credito comienzan con 3 digitos entre 100 y 299
                {
                    var creditCard = await _creditCardService.GetByIdAsync(emissorProduct.Id);

                    if (creditCard.Amount + svm.Cantity > creditCard.Limit)
                    {
                        return new TransactionStatusViewModel()
                        {
                            HasError = true,
                            Error = "Usted esta sobregirando la tarjeta."
                        };
                    }
                }

                else if (300 <= emissorIdentificator && emissorIdentificator <= 599) //Cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                {
                    var savingAccount = await _savingsAccountService.GetByIdAsync(emissorProduct.Id);

                    if (savingAccount.Amount - svm.Cantity < 0)
                    {
                        return new TransactionStatusViewModel()
                        {
                            HasError = true,
                            Error = "No tiene dinero suficiente"
                        };
                    }
                }

                if (100 <= receiverIdentificator && receiverIdentificator <= 299 && svm.Type != TransactionType.CreditCardPay)
                {
                    return new TransactionStatusViewModel()
                    {
                        HasError = true,
                        Error = "A las tarjetas de credito no se les deposita dinero"
                    };
                }
                #endregion
            }
            if (emissorProduct.Id == receiverProduct.Id)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Transferencia invalida."
                };
            }
            if (svm.Type == TransactionType.ExpressPay && emissorProduct.UserId == receiverProduct.UserId)
            {
                return new TransactionStatusViewModel()
                {
                    HasError = true,
                    Error = "Transferencia invalida."
                };
            }
            return new TransactionStatusViewModel()
            {
                HasError = false
            };
        }

        public async Task<bool> ProductExists(string Id)
        {
            var savingsAccount = await _savingsAccountService.FindAllAsync(x => x.Id == Id);
            var creditCard = await _creditCardService.FindAllAsync(x => x.Id == Id);
            var loan = await _loanService.FindAllAsync(x => x.Id == Id);
            if (savingsAccount.Count != 0 || creditCard.Count != 0 || loan.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<BaseProduct> GetProductByIdAsync(string Id)
        {
            BaseProduct product = new BaseProduct();
            if (Id != null)
            {
                if (Id.Length > 3)
                {
                    //Asumiendo que las tarjetas de credito comienzan con 3 digitos entre 100 y 299
                    if (100 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 299)
                    {
                        var entity = await _creditCardService.GetByIdAsync(Id);
                        if (entity != null)
                        {
                            product.Amount = entity.Amount;
                            product.Id = entity.Id;
                            product.UserId = entity.UserId;
                            product.CreatedById = entity.CreatedById;
                            product.CreatedDate = entity.CreatedDate;
                        }

                    }
                    //Asumiendo que las cuentas de ahorro comienzan con 3 digitos entre 300 y 599
                    else if (300 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 599)
                    {
                        var entity = await _savingsAccountService.GetByIdAsync(Id);
                        if (entity != null)
                        {
                            product.Amount = entity.Amount;
                            product.Id = entity.Id;
                            product.UserId = entity.UserId;
                            product.CreatedById = entity.CreatedById;
                            product.CreatedDate = entity.CreatedDate;
                        }
                    }
                    //Asumiendo que los préstamos comienzan con 3 digitos entre 600 y 999
                    else if (600 <= Convert.ToInt32(Id.Substring(0, 3)) && Convert.ToInt32(Id.Substring(0, 3)) <= 999)
                    {
                        var entity = await _loanService.GetByIdAsync(Id);
                        if (entity != null)
                        {
                            product.Amount = entity.Amount;
                            product.Id = entity.Id;
                            product.UserId = entity.UserId;
                            product.CreatedById = entity.CreatedById;
                            product.CreatedDate = entity.CreatedDate;
                        }
                    }
                    else
                    {
                        product = null;
                    }
                }
                else
                {
                    product = null;
                }
            }
            else
            {
                product = null;
            }
            return product;
        }

        public async Task<SaveBeneficiaryViewModel> AddBeneficiary(SaveBeneficiaryViewModel svm)
        {
            if (svm.BeneficiaryAccountId != null)
            {
                if (svm.BeneficiaryAccountId.Length >= 9)
                {
                    int Identificator = Convert.ToInt32(svm.BeneficiaryAccountId.Substring(0, 3));

                    if (!(300 <= Identificator && Identificator <= 599)) //Si no es una cuenta de ahorro
                    {
                        svm.HasError = true;
                        svm.Error = "Producto invalido.";
                    }
                    else if (await ProductExists(svm.BeneficiaryAccountId))
                    {
                        var existence = await _beneficiaryService.FindAllAsync(x => x.UserId == user.Id && x.BeneficiaryAccountId == svm.BeneficiaryAccountId);
                        if (existence.Count > 0)
                        {
                            svm.HasError = true;
                            svm.Error = "Este beneficiario ya esta registrado.";
                        }
                        else
                        {
                            svm = await _beneficiaryService.AddAsync(svm);
                        }
                    }
                    else
                    {
                        svm.HasError = true;
                        svm.Error = "Este producto no existe";
                    }
                }
                else
                {
                    svm.HasError = true;
                    svm.Error = "Este producto no existe";
                }
            }
            else
            {
                svm.HasError = true;
                svm.Error = "Inserte una cuenta.";
            }

            return svm;
        }

        public async Task DeleteBeneficiary(string Id)
        {
            var beneficiary = await _beneficiaryService.GetByIdAsync(Id);
            if (beneficiary != null)
            {
                await _beneficiaryService.Delete(beneficiary.Id);
            }
        }

        public async Task<List<TransactionViewModel>> GetTransactionHistorial()
        {
            var list = await _transactionService.GetAllAsync();
            var MyProducts = await GetAllProductsByClientAsync();

            var filteredTransactions = list.Where(t =>
                    MyProducts.SavingsAccounts.Any(p => p.Id == t.ReceiverProductId || p.Id == t.EmissorProductId) ||
                    MyProducts.CreditCards.Any(p => p.Id == t.ReceiverProductId || p.Id == t.EmissorProductId) ||
                    MyProducts.Loans.Any(p => p.Id == t.ReceiverProductId || p.Id == t.EmissorProductId)).ToList();
            return filteredTransactions;
        }

    }
}