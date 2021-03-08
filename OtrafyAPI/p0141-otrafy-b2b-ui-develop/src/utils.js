const utils = {
  thousandSeperator: num => {
    let parts = num.toString().split('.')
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, '.')
    return parts.join('.')
  },
  compareTime: (DateA, DateB) => {
    let a = new Date(DateA)
    let b = new Date(DateB)
    let msDateA = Date.UTC(a.getFullYear(), a.getMonth() + 1, a.getDate())
    let msDateB = Date.UTC(b.getFullYear(), b.getMonth() + 1, b.getDate())
    if (parseFloat(msDateA) < parseFloat(msDateB))
      return -1  // lt
    else if (parseFloat(msDateA) === parseFloat(msDateB))
      return 0  // eq
    else if (parseFloat(msDateA) > parseFloat(msDateB))
      return 1  // gt
    else
      return null // error
  },
}

export default utils