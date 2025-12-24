export default function GoogleLoginPage() {
    const handleLoginWithGoogle = async () => {
        const url = '/api/auth/oauth/google'
        const res = await fetch(url, { method: 'get' })
        const json = await res.json()
        const oauthUrl = json.url

        window.location.href = oauthUrl
    }

    return (
        <div style={styles.page}>
            <div style={styles.card}>
                <h2 style={styles.title}>Sign in</h2>
                <button onClick={handleLoginWithGoogle} style={styles.googleButton}>
                    <span style={styles.googleIcon}>G</span>
                    <span>Continue with Google</span>
                </button>
            </div>
        </div>
    );
}

const styles: Record<string, React.CSSProperties> = {
    page: {
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: "#f3f4f6",
    },
    card: {
        width: "360px",
        padding: "24px",
        borderRadius: "12px",
        backgroundColor: "#ffffff",
        boxShadow: "0 10px 25px rgba(0,0,0,0.1)",
        textAlign: "center",
    },
    title: {
        marginBottom: "20px",
        fontSize: "20px",
        fontWeight: 600,
    },
    googleButton: {
        width: "100%",
        height: "44px",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: "12px",
        borderRadius: "8px",
        border: "1px solid #d1d5db",
        backgroundColor: "#ffffff",
        cursor: "pointer",
        fontSize: "14px",
        fontWeight: 500,
    },
    googleIcon: {
        width: "20px",
        height: "20px",
        borderRadius: "50%",
        backgroundColor: "#4285F4",
        color: "#ffffff",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        fontWeight: 700,
        fontSize: "12px",
    },
};
