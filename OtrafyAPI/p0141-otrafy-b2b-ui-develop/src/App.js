import React, { useEffect } from 'react'
// Page
import LoginPage from './components/pages/LoginPage'
import ForgotPasswordPage from './components/pages/ForgotPasswordPage'
import HomePage from './components/pages/HomePage'
import NotFoundPage from './components/pages/NotFoundPage'
import ProtectedRoute from './components/pages/ProtectedRoute'
import CompanyManagementPage from './components/pages/CompanyManagementPage'
import RestrictedPage from './components/pages/RestrictedPage'
import ReportAnalyticsPage from './components/pages/ReportAnalyticsPage'
import DashboardPage from './components/pages/DashboardPage'
import SuppliersManagementPage from './components/pages/SuppliersManagementPage'
import AddSupplierPage from './components/pages/AddSupplierPage'
import SupportPage from './components/pages/SupportPage'
import NotificationsPage from './components/pages/NotificationsPage'
import CompanyDetailPage from './components/pages/CompanyDetailPage'
import TermsOfServicePage from './components/pages/TermsOfServicePage'
import PrivacyPolicyPage from './components/pages/PrivacyPolicyPage'
import ForgotPasswordSentSuccessPage from './components/pages/ForgotPasswordSentSuccessPage'
import ResetPasswordPage from './components/pages/ResetPasswordPage'
import CreateCompanyPage from './components/pages/CreateCompanyPage'
import CreateBuyerPage from './components/pages/CreateBuyerPage'
import MyProfilePage from './components/pages/MyProfilePage'
import InviteBuyerPage from './components/pages/InviteBuyerPage'
import AllFormsPage from './components/pages/AllFormsPage'
import CreateNewRequestPage from './components/pages/CreateNewRequestPage'
import CreateNewFormPage from './components/pages/CreateNewFormPage'
import BuyerInvitationDetailPage from './components/pages/BuyerInvitationDetailPage'
import SupplierDetailPage from './components/pages/SupplierDetailPage'
import AddProductPage from './components/pages/AddProductPage'
import EditFormPage from './components/pages/EditFormPage'
import ViewFormPage from './components/pages/ViewFormPage'
import RequestManagementPage from './components/pages/RequestManagementPage'
import RequestDetailPage from './components/pages/RequestDetailPage/RequestDetailPage'
// React Router
import { Router, Route, Switch } from 'react-router-dom'
import { createBrowserHistory } from 'history'
// Styling
import './App.less'
import ThemeProvider from './components/providers/ThemeProvider'
// MobX
import { Provider } from 'mobx-react'
import commonStore from './stores/commonStore'
import authStore from './stores/authStore'
import usersStore from './stores/usersStore'
import companiesStore from './stores/companiesStore'
import buyersStore from './stores/buyersStore'
import suppliersStore from './stores/suppliersStore'
import tagsStore from './stores/tagsStore'
import formsStore from './stores/formsStore'
import productsStore from './stores/productsStore'
import formsRequestsStore from './stores/formsRequestsStore'

const stores = {
  commonStore,
  authStore,
  usersStore,
  companiesStore,
  buyersStore,
  suppliersStore,
  tagsStore,
  formsStore,
  productsStore,
  formsRequestsStore,
}

const history = createBrowserHistory()

const App = () => {

  useEffect(() => {
    usersStore.setCurrentUser(history)
  }, [])

  useEffect(() => {
    commonStore.setTheme('default')
  }, [])

  return (
    <Provider {...stores}>
      <ThemeProvider>
        <Router history={history}>
          <Switch>
            <Route exact path="/terms-of-service" component={TermsOfServicePage}/>
            <Route exact path="/privacy-policy" component={PrivacyPolicyPage}/>
            <Route exact path="/login" component={LoginPage}/>
            <Route exact path="/invite/:invitedId" component={InviteBuyerPage}/>
            <Route exact path="/reset-password/:token" component={ResetPasswordPage}/>
            <Route exact path="/forgot-password" component={ForgotPasswordPage}/>
            <Route exact path='/forgot-password/success' component={ForgotPasswordSentSuccessPage}/>
            <ProtectedRoute exact path="/" component={HomePage}/>
            <ProtectedRoute exact path='/dashboard' component={DashboardPage}/>
            <ProtectedRoute exact path='/my-profile' component={MyProfilePage}/>
            <ProtectedRoute exact path="/company-management" component={CompanyManagementPage}/>
            <ProtectedRoute exact path="/company-management/create-new" component={CreateCompanyPage}/>
            <ProtectedRoute exact path="/company-management/:companyId" component={CompanyDetailPage}/>
            <ProtectedRoute exact path="/company-management/:companyId/create-buyer" component={CreateBuyerPage}/>
            <ProtectedRoute exact path="/company-management/buyer-detail/:buyerId" component={BuyerInvitationDetailPage}/>
            <ProtectedRoute exact path="/suppliers-management" component={SuppliersManagementPage}/>
            <ProtectedRoute exact path="/suppliers-management/add-new-supplier" component={AddSupplierPage}/>
            <ProtectedRoute exact path="/suppliers-management/suppliers-detail/:supplierId" component={SupplierDetailPage}/>
            <ProtectedRoute exact path="/suppliers-management/suppliers-detail/:supplierId/add-new-product" component={AddProductPage}/>
            <ProtectedRoute exact path="/suppliers-management/create-new-request" component={CreateNewRequestPage}/>
            <ProtectedRoute exact path="/reports-analytics" component={ReportAnalyticsPage}/>
            <ProtectedRoute exact path="/all-forms" component={AllFormsPage}/>
            <ProtectedRoute exact path="/all-forms/create-new-form" component={CreateNewFormPage}/>
            <ProtectedRoute exact path="/all-forms/edit-form-detail/:formId" component={EditFormPage}/>
            <ProtectedRoute exact path="/all-forms/view-form-detail/:formId" component={ViewFormPage}/>
            <ProtectedRoute exact path="/request-management" component={RequestManagementPage}/>
            <ProtectedRoute exact path="/request-management/request-detail/:formRequestId" component={RequestDetailPage}/>
            <ProtectedRoute exact path="/support" component={SupportPage}/>
            <ProtectedRoute exact path="/notifications" component={NotificationsPage}/>
            <Route exact path="/restricted" component={RestrictedPage}/>
            <Route component={NotFoundPage}/>
          </Switch>
        </Router>
      </ThemeProvider>
    </Provider>
  )
}

export default App
