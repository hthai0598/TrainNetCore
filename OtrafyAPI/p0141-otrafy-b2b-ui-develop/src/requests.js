import axios from 'axios'
import commonStore from './stores/commonStore'
import { apiUrl } from './config'

const requests = {
  get: (url, header = false) => {
    if (header) {
      return axios({
        method: `get`,
        url: `${apiUrl}${url}`,
        headers: {
          'Authorization': `Bearer ${commonStore.token}`,
        },
      })
    }
    return axios({
      method: `get`,
      url: `${apiUrl}${url}`,
    })
  },
  post: (url, body, header = false) => {
    if (header) {
      return axios({
        method: `post`,
        url: `${apiUrl}${url}`,
        headers: {
          'Authorization': `Bearer ${commonStore.token}`,
        },
        data: body,
      })
    }
    return axios({
      method: `post`,
      url: `${apiUrl}${url}`,
      data: body,
    })
  },
  delete: (url, header = false) => {
    if (header) {
      return axios({
        method: `delete`,
        url: `${apiUrl}${url}`,
        headers: {
          'Authorization': `Bearer ${commonStore.token}`,
        },
      })
    }
    return axios({
      method: `delete`,
      url: `${apiUrl}${url}`,
    })
  },
  put: (url, body, header = false) => {
    if (header) {
      return axios({
        method: `put`,
        url: `${apiUrl}${url}`,
        headers: {
          'Authorization': `Bearer ${commonStore.token}`,
        },
        data: body,
      })
    }
    return axios({
      method: `post`,
      url: `${apiUrl}${url}`,
      data: body,
    })
  },
}

const AuthRequest = {
  changePassword: (token, newPassword) =>
    requests.post(`/api/account/reset-password`, {
      tokenId: token,
      newPassword: newPassword,
    }),
  checkResetPasswordToken: token =>
    requests.post(`/api/account/valid-token`, {
      tokenId: token,
    }),
  login: (username, password) =>
    requests.post(`/api/auth/token`, {
      username: username,
      password: password,
    }),
  forgetPassword: email =>
    requests.post(`/api/account/forgot-password`, {
      email: email,
    }),
}

const BuyerRequest = {
  resendInvitation: buyerId =>
    requests.post(`/api/buyers/resend`, {
      buyerId: buyerId,
    }, true),
  getAll: params =>
    requests.get(`/api/buyers/get-all-buyers${params}`, true),
  createBuyerInvitation: body =>
    requests.post(`/api/buyers/create-buyer`, body, true),
  validateToken: token =>
    requests.post(`/api/account/valid-token`, {
      tokenId: token,
    }, true),
  activeBuyer: body =>
    requests.post(`/api/account/active-invite`, body, true),
  getStatistic: () =>
    requests.get(`/api/buyers/dashboard-statistical`, true),
}

const CompanyRequest = {
  updateInfo: (id, body) =>
    requests.put(`/api/company/${id}`, body, true),
  getAll: params =>
    requests.get(`/api/company${params}`, true),
  getById: id =>
    requests.get(`/api/company/${id}`, true),
  create: body =>
    requests.post(`/api/company`, body, true),
  getBuyerDetail: buyerId =>
    requests.get(`/api/company/get-buyer-detail?buyerId=${buyerId}`, true),
  updateBuyerDetail: (buyerId, body) =>
    requests.put(`/api/company/update-buyer/${buyerId}`, body, true),
}

const FormRequest = {
  createForm: data =>
    requests.post(`/api/buyers/create-form`, data, true),
  getAllForms: params =>
    requests.get(`/api/buyers/get-all-forms${params}`, true),
  getFormDetail: formId =>
    requests.get(`/api/buyers/get-form-detail?formId=${formId}`, true),
  updateFormDetail: (formId, data) =>
    requests.put(`/api/buyers/update-form/${formId}`, data, true),
}

const FormRequestRequest = {
  getRequestList: params =>
    requests.get(`/api/buyers/get-all-form-request-by-supplier${params}`, true),
  createRequest: info =>
    requests.post(`/api/buyers/create-form-request`, info, true),
  deleteRequest: (supplierId, formRequestId) =>
    requests.delete(`/api/buyers/delete-form-request/${formRequestId}?supplierId=${supplierId}`, true),
  getAllSupplierFormRequest: params =>
    requests.get(`/api/supplier/get-all-form-request${params}`, true),
  getFormRequestDetail: formRequestId =>
    requests.get(`/api/supplier/get-form-request-detail?formRequestId=${formRequestId}`, true),
  getAllRequestComments: params =>
    requests.get(`/api/supplier/get-all-comments${params}`, true),
  addComment: (formRequestId, comment) =>
    requests.put(`/api/supplier/add-comment-form-request/${formRequestId}`, comment, true),
}

const ProductRequest = {
  createProduct: info =>
    requests.post(`/api/supplier/create-product`, info, true),
  getAllProducts: params =>
    requests.get(`/api/supplier/get-all-products${params}`, true),
  getProductById: productId =>
    requests.get(`/api/supplier/get-product-detail?productId=${productId}`, true),
  updateProductDetail: (productId, info) =>
    requests.put(`/api/supplier/update-product/${productId}`, info, true),
}

const SupplierRequest = {
  create: body =>
    requests.post(`/api/supplier/create-supplier`, body, true),
  getAll: params =>
    requests.get(`/api/supplier${params}`, true),
  getSupplierDetail: supplierId =>
    requests.get(`/api/buyers/get-supplier-details?supplierId=${supplierId}`, true),
  updateSupplierDetail: (supplierId, body) =>
    requests.put(`/api/buyers/update-supplier/${supplierId}`, body, true),
}

const TagRequest = {
  getAll: params =>
    requests.get(`/api/buyers/tags${params}`, true),
  updateTags: tags =>
    requests.put(`/api/buyers/add-tag`, tags, true),
}

const UserRequest = {
  getCurrent: () =>
    requests.get(`/api/account/me`, true),
  updateProfiles: body =>
    requests.put(`/api/account/update-profile`, body, true),
}

export {
  AuthRequest,
  BuyerRequest,
  CompanyRequest,
  FormRequest,
  FormRequestRequest,
  ProductRequest,
  SupplierRequest,
  TagRequest,
  UserRequest,
}
