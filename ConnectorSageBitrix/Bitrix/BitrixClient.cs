﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ConnectorSageBitrix.Logging;

namespace ConnectorSageBitrix.Bitrix
{
    public class BitrixClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly Logger _logger;

        public BitrixClient(string baseUrl, Logger logger)
        {
            _baseUrl = baseUrl;
            _logger = logger;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        #region Socios

        public async Task<List<BitrixSocio>> ListSociosAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeSocios
            };

            _logger.Debug("Requesting socios list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixSocio>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixSocio>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} socios from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateSocioAsync(BitrixSocio socio)
        {
            var body = new
            {
                fields = new
                {
                    title = socio.Title,
                    ufCrm55Dni = socio.DNI,
                    ufCrm55Cargo = socio.Cargo,
                    ufCrm55Admin = socio.Administrador,
                    ufCrm55Participacion = socio.Participacion,
                    ufCrm55RazonSocial = socio.RazonSocialEmpleado // Nuevo campo
                },
                entityTypeId = BitrixConstants.EntityTypeSocios
            };

            _logger.Debug($"Creating socio with DNI: {socio.DNI}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created socio with ID: {id}");

            return id;
        }

        public async Task UpdateSocioAsync(int id, BitrixSocio socio)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = socio.Title,
                    ufCrm55Dni = socio.DNI,
                    ufCrm55Cargo = socio.Cargo,
                    ufCrm55Admin = socio.Administrador,
                    ufCrm55Participacion = socio.Participacion,
                    ufCrm55RazonSocial = socio.RazonSocialEmpleado
                },
                entityTypeId = BitrixConstants.EntityTypeSocios
            };

            _logger.Debug($"Updating socio with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated socio with ID: {id}");
        }

        #endregion

        #region Cargos

        public async Task<List<BitrixCargo>> ListCargosAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeCargos
            };

            _logger.Debug("Requesting cargos list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixCargo>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixCargo>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} cargos from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateCargoAsync(BitrixCargo cargo)
        {
            var body = new
            {
                fields = new
                {
                    title = cargo.Title,
                    ufCrm57Dni = cargo.DNI, // Usar DNI en lugar de GuidPersona
                    ufCrm57Cargo = cargo.Cargo,
                    ufCrm57Unico = cargo.SocioUnico,
                    ufCrm57Caducidad = cargo.Caducidad,
                    ufCrm57RazonSocial = cargo.RazonSocialEmpleado
                },
                entityTypeId = BitrixConstants.EntityTypeCargos
            };

            _logger.Debug($"Creating cargo with DNI: {cargo.DNI}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created cargo with ID: {id}");

            return id;
        }

        public async Task UpdateCargoAsync(int id, BitrixCargo cargo)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = cargo.Title,
                    ufCrm57Dni = cargo.DNI, // Usar DNI en lugar de GuidPersona
                    ufCrm57Cargo = cargo.Cargo,
                    ufCrm57Unico = cargo.SocioUnico,
                    ufCrm57Caducidad = cargo.Caducidad,
                    ufCrm57RazonSocial = cargo.RazonSocialEmpleado
                },
                entityTypeId = BitrixConstants.EntityTypeCargos
            };

            _logger.Debug($"Updating cargo with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated cargo with ID: {id}");
        }

        #endregion

        #region Actividades

        public async Task<List<BitrixActividad>> ListActividadesAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeActividades
            };

            _logger.Debug("Requesting actividades list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixActividad>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixActividad>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} actividades from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateActividadAsync(BitrixActividad actividad)
        {
            var body = new
            {
                fields = new
                {
                    title = actividad.Title,
                    ufCrm59Guidactividad = actividad.GuidActividad,
                    ufCrm59Descripcion = actividad.Descripcion,
                    ufCrm_59_CNAE_93 = actividad.CNAE93,
                    ufCrm59AltaIae = actividad.AltaIAE,
                    ufCrm59BajaIae = actividad.BajaIAE,
                    ufCrm59Epigrafe = actividad.Epigrafe,
                    ufCrm_59_CNAE_09 = actividad.CNAE09,
                    ufCrm59Sufijo = actividad.Sufijo,
                    ufCrm59Principal = actividad.Principal,
                    ufCrm59TipoEpigrafe = actividad.TipoEpigrafe
                },
                entityTypeId = BitrixConstants.EntityTypeActividades
            };

            _logger.Debug($"Creating actividad with GuidActividad: {actividad.GuidActividad}");

            _logger.Debug($"BODY: {body}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created actividad with ID: {id}");

            return id;
        }

        public async Task UpdateActividadAsync(int id, BitrixActividad actividad)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = actividad.Title,
                    ufCrm59Guidactividad = actividad.GuidActividad,
                    ufCrm59Descripcion = actividad.Descripcion,
                    ufCrm_59_CNAE_93 = actividad.CNAE93,
                    ufCrm59AltaIae = actividad.AltaIAE,
                    ufCrm59BajaIae = actividad.BajaIAE,
                    ufCrm59Epigrafe = actividad.Epigrafe,
                    ufCrm_59_CNAE_09 = actividad.CNAE09,
                    ufCrm59Sufijo = actividad.Sufijo,
                    ufCrm59Principal = actividad.Principal,
                    ufCrm59TipoEpigrafe = actividad.TipoEpigrafe
                },
                entityTypeId = BitrixConstants.EntityTypeActividades
            };

            _logger.Debug($"Updating actividad with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated actividad with ID: {id}");
        }

        #endregion

        #region Modelos

        public async Task<List<BitrixModelo>> ListModelosAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeModelos
            };

            _logger.Debug("Requesting modelos list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixModelo>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixModelo>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} modelos from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateModeloAsync(BitrixModelo modelo)
        {
            var body = new
            {
                fields = new
                {
                    title = modelo.Title,
                    ufCrm133Codigo = modelo.CodigoModeloImp,
                    ufCrm133Periodicidad = modelo.Periodicidad,
                    begindate = modelo.FechaInicio,
                    closedate = modelo.FechaCierre,
                    ufCrm133Estado = modelo.Estado
                },
                entityTypeId = BitrixConstants.EntityTypeModelos
            };

            _logger.Debug($"Creating modelo with codigo: {modelo.CodigoModeloImp}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created modelo with ID: {id}");

            return id;
        }

        public async Task UpdateModeloAsync(int id, BitrixModelo modelo)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = modelo.Title,
                    ufCrm133Codigo = modelo.CodigoModeloImp,
                    ufCrm133Periodicidad = modelo.Periodicidad,
                    begindate = modelo.FechaInicio,
                    closedate = modelo.FechaCierre,
                    ufCrm133Estado = modelo.Estado
                },
                entityTypeId = BitrixConstants.EntityTypeModelos
            };

            _logger.Debug($"Updating modelo with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated modelo with ID: {id}");
        }

        #endregion

        #region Helper Methods

        private async Task<T> DoRequestAsync<T>(string method, object body)
        {
            string url = $"{_baseUrl}{method}";
            _logger.Debug($"Making request to: {url}");

            string jsonBody = JsonConvert.SerializeObject(body);
            _logger.Debug($"Request body: {jsonBody}");
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Invalid status code: {response.StatusCode}, body: {responseContent}");
            }

            if (string.IsNullOrEmpty(responseContent) || responseContent == "[]")
            {
                _logger.Debug("Received empty response");
                return default;
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        #endregion

        #region Companies

        public async Task<List<BitrixCompany>> ListCompaniesAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeCompanies
            };

            _logger.Debug("Requesting companies list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixCompany>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixCompany>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} companies from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateCompanyAsync(BitrixCompany company)
        {
            var body = new
            {
                fields = new
                {
                    title = company.Title,
                    ufCrmCompanyCategoria = company.CodigoCategoriaCliente,
                    ufCrmCompanyRazon = company.RazonSocial,
                    ufCrmCompanyDivisa = company.CodigoDivisa,
                    ufCrmCompanyDomicilio = company.Domicilio,
                    ufCrmCompanyDomicilio2 = company.Domicilio2,
                    ufCrmCompanyMunicipio = company.Municipio,
                    ufCrmCompanyCodigoPostal = company.CodigoPostal,
                    ufCrmCompanyProvincia = company.Provincia,
                    ufCrmCompanyNacion = company.Nacion,
                    ufCrmCompanyCodigoNacion = company.CodigoNacion,
                    ufCrmCompanyTelefono = company.Telefono,
                    ufCrmCompanyEmail = company.EMail1
                },
                entityTypeId = BitrixConstants.EntityTypeCompanies
            };

            _logger.Debug($"Creating company with codigo: {company.CodigoCategoriaCliente}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created company with ID: {id}");

            return id;
        }

        public async Task UpdateCompanyAsync(int id, BitrixCompany company)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = company.Title,
                    ufCrmCompanyCategoria = company.CodigoCategoriaCliente,
                    ufCrmCompanyRazon = company.RazonSocial,
                    ufCrmCompanyDivisa = company.CodigoDivisa,
                    ufCrmCompanyDomicilio = company.Domicilio,
                    ufCrmCompanyDomicilio2 = company.Domicilio2,
                    ufCrmCompanyMunicipio = company.Municipio,
                    ufCrmCompanyCodigoPostal = company.CodigoPostal,
                    ufCrmCompanyProvincia = company.Provincia,
                    ufCrmCompanyNacion = company.Nacion,
                    ufCrmCompanyCodigoNacion = company.CodigoNacion,
                    ufCrmCompanyTelefono = company.Telefono,
                    ufCrmCompanyEmail = company.EMail1
                },
                entityTypeId = BitrixConstants.EntityTypeCompanies
            };

            _logger.Debug($"Updating company with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated company with ID: {id}");
        }

        #endregion

        #region Products

        public async Task<List<BitrixProduct>> ListProductsAsync()
        {
            var body = new
            {
                entityTypeId = BitrixConstants.EntityTypeProducts
            };

            _logger.Debug("Requesting products list from Bitrix24");

            var response = await DoRequestAsync<BitrixItemListResponse<BitrixProduct>>("crm.item.list", body);

            if (response?.Result?.Items == null)
            {
                return new List<BitrixProduct>();
            }

            _logger.Debug($"Retrieved {response.Result.Items.Count} products from Bitrix24");
            return response.Result.Items;
        }

        public async Task<int> CreateProductAsync(BitrixProduct product)
        {
            var body = new
            {
                fields = new
                {
                    title = product.Title,
                    ufCrmProductCodigo = product.CodigoArticulo,
                    ufCrmProductDescripcion = product.DescripcionArticulo,
                    ufCrmProductObsoleto = product.ObsoletoLc,
                    ufCrmProductLinea = product.DescripcionLinea,
                    ufCrmProductPrecio = product.PrecioVenta,
                    ufCrmProductDivisa = product.CodigoDivisa,
                    ufCrmProductIvaIncluido = product.IvaIncluido
                },
                entityTypeId = BitrixConstants.EntityTypeProducts
            };

            _logger.Debug($"Creating product with codigo: {product.CodigoArticulo}");

            var response = await DoRequestAsync<BitrixItemAddResponse>("crm.item.add", body);

            int id = response?.Result?.Item?.ID ?? 0;
            _logger.Debug($"Created product with ID: {id}");

            return id;
        }

        public async Task UpdateProductAsync(int id, BitrixProduct product)
        {
            var body = new
            {
                id = id,
                fields = new
                {
                    title = product.Title,
                    ufCrmProductCodigo = product.CodigoArticulo,
                    ufCrmProductDescripcion = product.DescripcionArticulo,
                    ufCrmProductObsoleto = product.ObsoletoLc,
                    ufCrmProductLinea = product.DescripcionLinea,
                    ufCrmProductPrecio = product.PrecioVenta,
                    ufCrmProductDivisa = product.CodigoDivisa,
                    ufCrmProductIvaIncluido = product.IvaIncluido
                },
                entityTypeId = BitrixConstants.EntityTypeProducts
            };

            _logger.Debug($"Updating product with ID: {id}");

            await DoRequestAsync<BitrixUpdateResponse>("crm.item.update", body);

            _logger.Debug($"Successfully updated product with ID: {id}");
        }

        #endregion
    }

    #region Response Types

    public class BitrixItemListResponse<T>
    {
        [JsonProperty("result")]
        public BitrixItemListResult<T> Result { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class BitrixItemListResult<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }

    public class BitrixItemAddResponse
    {
        [JsonProperty("result")]
        public BitrixItemAddResult Result { get; set; }
    }

    public class BitrixItemAddResult
    {
        [JsonProperty("item")]
        public BitrixItemId Item { get; set; }
    }

    public class BitrixItemId
    {
        [JsonProperty("id")]
        public int ID { get; set; }
    }

    public class BitrixUpdateResponse
    {
        [JsonProperty("result")]
        public BitrixUpdateResult Result { get; set; }
    }

    public class BitrixUpdateResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    #endregion
}