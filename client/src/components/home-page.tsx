const HomePage = () => {
    const userInfo = JSON.parse(localStorage.getItem('userInfo') ?? '{}')

    return (
        <div>
            <div style={styles.container}>
                <span>Hello <b>{userInfo.name}</b></span>
                <img src={userInfo.picture} style={styles.image} referrerPolicy="no-referrer" />
            </div>
            <p>Email: <b>{userInfo.email}</b></p>
        </div>
    )
}

export default HomePage

const styles: Record<string, React.CSSProperties> = {
    container: {
        display: 'flex',
        justifyContent: 'start',
        alignItems: 'center',
    },
    image: {
        borderRadius: '50%',
        height: '30px',
        marginLeft: '6px',
    }
}
