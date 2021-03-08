const {override, fixBabelImports, addLessLoader, disableEsLint, addDecoratorsLegacy} = require("customize-cra")

module.exports = override(
    disableEsLint(),
    addDecoratorsLegacy(),
    fixBabelImports("import", {
      libraryName: "antd",
      libraryDirectory: "es",
      style: true
    }),
    addLessLoader({
      javascriptEnabled: true,
      modifyVars: {
        // "@primary-color": "#3DBEA3",
        "@layout-header-height": "50px"
      }
    })
)
