import { observable, action, toJS } from 'mobx'

class CommonStore {

  @observable token = localStorage.getItem('jwt')
  @observable tokenExpiration = localStorage.getItem('tokenExpiration')
  @observable isLoading = false
  @observable isRemember = true
  @observable isDrawerVisible = false
  @observable isSidebarCollapse = false
  @observable mouseCordinate = {
    x: 0,
    y: 0,
  }
  @observable appTheme = {
    name: 'default',
    solidColor: '#3DBEA3',
    solidLightColor: '#ecf9f6',
    gradientColor: 'linear-gradient(167.51deg, #2ECF94 24.37%, #3DBEA3 78.07%)',
    shadowColor: '0 2px 10px rgba(46,207,148,0.6)',
  }

  @action setMouseCordinate(e) {
    this.mouseCordinate.x = e.clientX
    this.mouseCordinate.y = e.clientY
  }

  @action setToken(token) {
    this.token = token
  }

  @action setTokenExpiration(date) {
    this.tokenExpiration = date
  }

  @action checkToken() {
    return !!(localStorage.jwt || sessionStorage.jwt)
  }

  @action clearToken() {
    localStorage.clear()
    sessionStorage.clear()
  }

  @action setRemember(isRemember) {
    this.isRemember = isRemember
  }

  @action setLoadingProgress(state) {
    this.isAppLoading = state
  }

  @action toggleCollapseSidebar(state) {
    this.isSidebarCollapse = state
  }

  @action toggleDrawer(state) {
    this.isDrawerVisible = state
  }

  @action setTheme(color) {
    switch (color) {
      case 'default':
        this.appTheme.name = 'green'
        this.appTheme.solidColor = '#3DBEA3'
        this.appTheme.solidLightColor = '#ecf9f6'
        this.appTheme.gradientColor = 'linear-gradient(167.51deg, #2ECF94 24.37%, #3DBEA3 78.07%)'
        this.appTheme.shadowColor = '0 2px 10px rgba(46,207,148,0.6)'
        break
      case 'pink':
        this.appTheme.name = 'pink'
        this.appTheme.solidColor = 'rgb(244, 67, 54)'
        this.appTheme.solidLightColor = 'rgb(254, 237, 235)'
        this.appTheme.gradientColor = 'linear-gradient(108.84deg, #F77062 0%, #FE5196 100%)'
        this.appTheme.shadowColor = '0 2px 10px rgba(254, 81, 150, 0.5)'
        break
      case 'blue':
        this.appTheme.name = 'blue'
        this.appTheme.solidColor = 'rgb(33, 150, 243)'
        this.appTheme.solidLightColor = 'rgb(233, 245, 254)'
        this.appTheme.gradientColor = 'linear-gradient(108.88deg, #04BEFE 0%, #4481EB 100%)'
        this.appTheme.shadowColor = '0 2px 10px rgba(68, 129, 235, 0.5)'
        break
      case 'csstricks':
        this.appTheme.name = 'csstricks'
        this.appTheme.solidColor = 'rgb(229,46,113)'
        this.appTheme.solidLightColor = 'rgba(229,46,113, .2)'
        this.appTheme.gradientColor = 'linear-gradient(to top left,#ff8a00,#e52e71)'
        this.appTheme.shadowColor = '0px 2px 10px rgba(229,46,113, 0.5)'
        break
      case 'black':
        this.appTheme.name = 'black'
        this.appTheme.solidColor = 'rgb(70, 70, 70)'
        this.appTheme.solidLightColor = 'rgb(200, 200, 200)'
        this.appTheme.gradientColor = 'linear-gradient(108.88deg, rgb(121, 121, 121) 0%, rgb(70, 70, 70) 100%)'
        this.appTheme.shadowColor = '0px 2px 10px rgba(70, 70, 70, 0.5)'
    }
  }
}

export default new CommonStore()
