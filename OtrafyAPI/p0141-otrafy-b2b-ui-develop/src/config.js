const merge = require("lodash/merge")

const config = {
  all: {
    env: process.env.NODE_ENV || "development",
    isDev: process.env.NODE_ENV !== "production",
    basename: process.env.PUBLIC_URL,
    isBrowser: typeof window !== "undefined"
  },
  test: {},
  development: {
    apiUrl: process.env.API_URL || "http://117.6.135.148:8558"
  },
  production: {
    apiUrl: process.env.API_URL || "http://117.6.135.148:8558"
  }
}

module.exports = merge(config.all, config[config.all.env])
